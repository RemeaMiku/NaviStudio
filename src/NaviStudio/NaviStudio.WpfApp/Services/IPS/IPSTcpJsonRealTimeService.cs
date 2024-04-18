using System.Diagnostics;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using NaviStudio.Shared;
using NaviStudio.Shared.Models.Options;
using NaviStudio.Shared.Serialization;
using NaviStudio.WpfApp.Services.Contracts;

namespace NaviStudio.WpfApp.Services;

public class IPSTcpJsonRealTimeService : IRealTimeService
{
    #region Public Events

    public event EventHandler<EpochData?>? EpochDataReceived;

    #endregion Public Events

    #region Public Properties

    public bool IsRunning { get; private set; }

    #endregion Public Properties

    #region Public Methods

    static void WriteOptFile(InputOptions options, int serverMode, int roveType, int baseType, string filePath)
    {
        if(string.IsNullOrEmpty(filePath))
            return;
        var content =
            $"""
            # Real-Time IPS Engine Caster
            # ZMQ_Address构成: Type,Mode,ID,Data (2010)
            # 目前仅支持OEM4格式GNSS+IMU组合观测值输出

            RTIE_ServerMode = {serverMode}                                         ;<0:rove, 1:base, 2:correction>
            RTIE_ServerType_Rove = {roveType}                                    ;<0:Serial, 1:TCP, 2:ntrip>
            RTIE_ServerType_Base = {baseType}                                    ;<0:Serial, 1:TCP, 2:ntrip>
            RTIE_ZMQ_ID = {options.ZmqId}                                             ;<测站编号, 流动站/基站ID>
            RTIE_BaseSiteType = {(int)options.BaseType}                                       ;<基站类型, 0:单基站, 1:VRS>
            RTIE_StreamFormat = {(int)options.Format}                                       ;<数据类型, 0:STRFMT_RTCM2, 1:STRFMT_RTCM3, 2:STRFMT_OEM4, 4:STRFMT_UBX>
            RTIE_ServerCycle = {options.ServerCycle}                                     ;<数据率ms, 1000, 20, 10, 5>
            RTIE_ServerIMUType = "{options.ImuType}"                                 ;<惯导类型, "NULL", "NovAtel SPAN FSAS", ...>

            # Serial
            RTIE_Serial_Port = {options.SerialOptions.PortName}                                     ;<COM6,ttyUSB6>
            RTIE_Serial_Bitrate = {options.SerialOptions.BaudRate}                                ;<300,600,1200,2400,4800,9600,19200,38400,57600,115200,230400>
            RTIE_Serial_ByteSize = {options.SerialOptions.DataBits}                                    ;<7, 8>
            RTIE_Serial_Parity = {options.SerialOptions.Parity.ToString()[0]}                                      ;<N:None, E:Even, O:Odd>
            RTIE_Serial_StopBits = {(int)options.SerialOptions.StopBits}                                    ;<1, 2>
            RTIE_Serial_FlowControl = {(options.SerialOptions.RtsEnable ? "RTS" : "OFF")}                               ;<OFF:None, RTS:RTS/CTS>

            # TCP
            RTIE_TCP_Port = {options.TcpOptions.Port}                                            ;
            RTIE_TCP_Address = {options.TcpOptions.Address}                                         ;

            # NTRIP
            RTIE_NTRIP_CasterHost = {options.NtripOptions.CasterHost}                    ;
            RTIE_NTRIP_Mountpoint = {options.NtripOptions.MountPoint}                          ;
            RTIE_NTRIP_Port = {options.NtripOptions.Port}                                      ;
            RTIE_NTRIP_UserID = {options.NtripOptions.UserName}                                 ;
            RTIE_NTRIP_Password = {options.NtripOptions.Password}                             ;

            # Send Sol
            RTIE_SendSol_Type = 2                                       ;<0:Serial, 1:TCP/IP, 2:ZMQ>
            RTIE_SendSol_Serial_Port = COM6                             ;<COM6,ttyUSB6>
            RTIE_SendSol_Serial_Bitrate = 115200                        ;<300,600,1200,2400,4800,9600,19200,38400,57600,115200,230400>
            RTIE_SendSol_Serial_ByteSize = 8                            ;<7, 8>
            RTIE_SendSol_Serial_Parity = N                              ;<N:None, E:Even, O:Odd>
            RTIE_SendSol_Serial_StopBits = 1                            ;<1, 2>
            RTIE_SendSol_Serial_FlowControl = OFF                       ;<OFF:None, RTS:RTS/CTS>
            RTIE_SendSol_TCP_Port =                                     ;
            RTIE_SendSol_TCP_Address =                                  ;
            RTIE_SendSol_ZMQ_Address =                                  ;

            # GNSS Switch Freq
            GNSS_GPSOrder = 0 1 2                                                  ; [0,1,2,3,4,5,,] <将rtklib的obs，按指定顺序塞入到IPS的obs中>
            GNSS_GLOOrder = 0 1 2                                                  ; [0,1,2,3,4,5,,] <将rtklib的obs，按指定顺序塞入到IPS的obs中>
            GNSS_BD2Order = 0 3 2                                                  ; [0,1,2,3,4,5,,] <将rtklib的obs，按指定顺序塞入到IPS的obs中>
            GNSS_BD3Order = 4 3 2                                                  ; [0,1,2,3,4,5,,] <将rtklib的obs，按指定顺序塞入到IPS的obs中>
            GNSS_GALOrder = 0 1 2                                                  ; [0,1,2,3,4,5,,] <将rtklib的obs，按指定顺序塞入到IPS的obs中>
            GNSS_QZSOrder = 0 1 2                                                  ; [0,1,2,3,4,5,,] <将rtklib的obs，按指定顺序塞入到IPS的obs中>

            RTIE_END;


            # NTRIP
            RTIE_NTRIP_CasterHost = rtk.ntrip.qxwz.com                  ;
            RTIE_NTRIP_Mountpoint = RTCM32_GGB                          ;
            RTIE_NTRIP_Port = 8002                                      ;
            RTIE_NTRIP_UserID = qxdvdm0017                              ;
            RTIE_NTRIP_Password = 64e26d9                               ;


            # NTRIP
            RTIE_NTRIP_CasterHost = rtk.ntrip.qxwz.com                  ;
            RTIE_NTRIP_Mountpoint = RTCM32_GGB                          ;
            RTIE_NTRIP_Port = 8002                                      ;
            RTIE_NTRIP_UserID = qxdvdm0020                              ;
            RTIE_NTRIP_Password = 1234567                               ;

            # NTRIP
            RTIE_NTRIP_CasterHost = ntrip.gnsslab.cn                    ;
            RTIE_NTRIP_Mountpoint = CHPG00BRA0                          ;
            RTIE_NTRIP_Port = 2101                                      ;
            RTIE_NTRIP_UserID = WHUCJin                                 ;
            RTIE_NTRIP_Password = admin1202                             ;
            """;
        File.WriteAllText(filePath, content);
    }

    static void WriteOptFiles(RealTimeOptions options, string baseOptionFilePath, string roveOptionFilePath)
    {
        var roveType = (int)options.RoverOptions.Type;
        var baseType = (int)options.BaseOptions.Type;
        WriteOptFile(options.RoverOptions, 0, roveType, baseType, roveOptionFilePath);
        WriteOptFile(options.BaseOptions, 1, roveType, baseType, baseOptionFilePath);
    }

    DateTime _lastReceivedTime = DateTime.MaxValue;

    readonly List<Process> _processes = [];

    public async Task StartAsync(RealTimeOptions options, CancellationToken token)
    {
        if(IsRunning)
            throw new InvalidOperationException("It's already started.");
        IsRunning = true;
        _lastReceivedTime = DateTime.UtcNow;
        _processes.Clear();
        try
        {
            var settings = App.Current.SettingsManager.Settings.SolutionSettings;
            WriteOptFiles(options, settings.BaseOptionFilePath, settings.RoverOptionFilePath);
            using var listener = new TcpListener(settings.EpochDataTcpOptions.ToIPEndPoint());
            listener.Start();
            if(settings.IsSolverEnabled)
                settings.SolverProcessPaths.ForEach(path => _processes.Add(Process.Start(path)));
            using var client = await listener.AcceptTcpClientAsync(token);
            using var tcpStream = client.GetStream();
            var jsonOptions = new JsonSerializerOptions()
            {
                IgnoreReadOnlyProperties = true,
                Converters = { new UtcTimeJsonConverter() }
            };
            try
            {
                while(!token.IsCancellationRequested && DateTime.UtcNow - _lastReceivedTime <= TimeSpan.FromSeconds(settings.Timeout))
                {
                    var bytes = new byte[10240];
                    var count = await tcpStream.ReadAsync(bytes, token);
                    if(count == 0)
                        continue;
                    var json = Encoding.UTF8.GetString(bytes[0..count]);
                    if(string.IsNullOrEmpty(json))
                        continue;
                    var epochData = JsonSerializer.Deserialize<EpochData>(json, jsonOptions);
                    EpochDataReceived?.Invoke(this, epochData);
                    Trace.WriteLine($"Received:{DateTime.Now}");
                    _lastReceivedTime = DateTime.UtcNow;
                }
            }
            catch(OperationCanceledException) { }
        }
        catch(TaskCanceledException) { }
        catch(OperationCanceledException) { }
        finally
        {
            _processes.ForEach(p => p.Kill());
            IsRunning = false;
        }
    }

    #endregion Public Methods
}
