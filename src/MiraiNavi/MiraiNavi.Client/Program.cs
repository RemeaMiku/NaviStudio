using System.Net.Sockets;
using System.Text.Json;
using MiraiNavi.Shared.Models;
using MiraiNavi.Shared.Serialization;
using NaviSharp;

using var client = new TcpClient();
client.Connect(RealTimeControlOptions.DefaultIPEndPoint);
Console.WriteLine("Client Connected");
using var stream = client.GetStream();
using var writer = new StreamWriter(stream) { AutoFlush = true };
using var reader = new StreamReader("D:\\RemeaMiku study\\course in progress\\Graduation\\data\\机载.dts");
reader.ReadLine();
reader.ReadLine();
var options = new JsonSerializerOptions()
{
    IgnoreReadOnlyProperties = true,
    WriteIndented = true,
};
options.Converters.Add(new UtcTimeJsonConverter());
options.Converters.Add(new Vector3JsonConverter());
while (!reader.EndOfStream)
{
    var line = reader.ReadLine();
    var epochData = ParseLine(line!);
    writer.WriteLine(JsonSerializer.Serialize(epochData, options));
    Thread.Sleep(500);
}
writer.WriteLine("END");
client.Close();

static EpochData ParseLine(string line)
{
    ArgumentNullException.ThrowIfNull(line);
    var timeStamp = UtcTime.ParseExact(line[..23], "yyyy/MM/dd HH:mm:ss.fff", null);
    var ecef_x = double.Parse(line[44..58]);
    var ecef_y = double.Parse(line[59..73]);
    var ecef_z = double.Parse(line[74..88]);
    var lat = Angle.FromDegrees(int.Parse(line[90..93]), byte.Parse(line[94..96]), double.Parse(line[97..106]));
    var lng = Angle.FromDegrees(int.Parse(line[108..112]), byte.Parse(line[113..115]), double.Parse(line[116..125]));
    var hgt = double.Parse(line[127..138]);
    var bl_e = double.Parse(line[176..188]);
    var bl_n = double.Parse(line[190..202]);
    var bl_u = double.Parse(line[204..216]);
    var std_bl_e = float.Parse(line[218..230]);
    var std_bl_n = float.Parse(line[232..244]);
    var std_bl_u = float.Parse(line[246..258]);
    var std_bl_en = float.Parse(line[260..272]);
    var std_bl_eu = float.Parse(line[274..286]);
    var std_bl_nu = float.Parse(line[288..300]);
    var ecef_vx = double.Parse(line[302..312]);
    var ecef_vy = double.Parse(line[314..324]);
    var ecef_vz = double.Parse(line[326..336]);
    var vel_e = double.Parse(line[338..348]);
    var vel_n = double.Parse(line[350..360]);
    var vel_u = double.Parse(line[362..372]);
    var std_vel_e = float.Parse(line[374..386]);
    var std_vel_n = float.Parse(line[388..400]);
    var std_vel_u = float.Parse(line[402..414]);
    var std_vel_en = float.Parse(line[416..428]);
    var std_vel_eu = float.Parse(line[430..442]);
    var std_vel_nu = float.Parse(line[444..456]);
    var yaw = Angle.FromDegrees(double.Parse(line[458..467]));
    var pitch = Angle.FromDegrees(double.Parse(line[469..478]));
    var roll = Angle.FromDegrees(double.Parse(line[480..489]));
    var std_yaw = Angle.FromDegrees(double.Parse(line[491..503]));
    var std_pitch = Angle.FromDegrees(double.Parse(line[505..517]));
    var std_roll = Angle.FromDegrees(double.Parse(line[519..531]));
    var std_yaw_pitch = Angle.FromDegrees(double.Parse(line[533..545]));
    var std_yaw_roll = Angle.FromDegrees(double.Parse(line[547..559]));
    var std_pitch_roll = Angle.FromDegrees(double.Parse(line[561..573]));
    var accBias_x = float.Parse(line[575..588]);
    var accBias_y = float.Parse(line[590..603]);
    var accBias_z = float.Parse(line[605..618]);
    var std_accBias_x = float.Parse(line[620..633]);
    var std_accBias_y = float.Parse(line[635..648]);
    var std_accBias_z = float.Parse(line[650..663]);
    var gyroBias_x = float.Parse(line[665..678]);
    var gyroBias_y = float.Parse(line[680..693]);
    var gyroBias_z = float.Parse(line[695..708]);
    var std_gyroBias_x = float.Parse(line[710..723]);
    var std_gyroBias_y = float.Parse(line[725..738]);
    var std_gyroBias_z = float.Parse(line[740..753]);
    var hdop = float.Parse(line[1078..1084]);
    var vdop = float.Parse(line[1086..1092]);
    var ratio = float.Parse(line[1106..1113]);
    return new()
    {
        TimeStamp = timeStamp,
        Pose = new()
        {
            TimeStamp = timeStamp,
            EcefCoord = new(ecef_x, ecef_y, ecef_z),
            GeodeticCoord = new(lat, lng, hgt),
            EastVelocity = vel_e,
            NorthVelocity = vel_n,
            UpVelocity = vel_u,
            XVelocity = ecef_vx,
            YVelocity = ecef_vy,
            ZVelocity = ecef_vz,
            EulerAngles = new(yaw, pitch, roll)
        },
        EastBaseLine = bl_e,
        NorthBaseLine = bl_n,
        UpBaseLine = bl_u,
        ImuBias = new()
        {
            TimeStamp = timeStamp,
            AccelerometerBias = new(accBias_x, accBias_y, accBias_z),
            GyroscopeBias = new(gyroBias_x, gyroBias_y, gyroBias_z)
        },
        PosePrecision = new()
        {
            TimeStamp = timeStamp,
            Ratio = ratio,
            Hdop = hdop,
            Vdop = vdop,
            StdEastVelocity = std_vel_e,
            StdNorthVelocity = std_vel_n,
            StdUpVelocity = std_vel_u,
            StdEastNorthVelocity = std_vel_en,
            StdEastUpVelocity = std_vel_eu,
            StdNorthUpVelocity = std_vel_nu,
            StdYaw = std_yaw,
            StdPitch = std_pitch,
            StdRoll = std_roll,
            StdYawPitch = std_yaw_pitch,
            StdYawRoll = std_yaw_roll,
            StdPitchRoll = std_pitch_roll
        },
        BaseLinePrecision = new()
        {
            TimeStamp = timeStamp,
            StdEast = std_bl_e,
            StdNorth = std_bl_n,
            StdUp = std_bl_u,
            StdEastNorth = std_bl_en,
            StdEastUp = std_bl_eu,
            StdNorthUp = std_bl_nu
        },
        ImuBiasPrecision = new()
        {
            TimeStamp = timeStamp,
            StdAccelerometerBias = new(std_accBias_x, std_accBias_y, std_accBias_z),
            StdGyroscopeBias = new(std_gyroBias_x, std_gyroBias_y, std_gyroBias_z)
        }
    };
}