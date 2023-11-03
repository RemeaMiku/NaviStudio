namespace MiraiNavi.WpfApp.Views;

public interface IHasViewModel<TViewModel>
{
    public TViewModel ViewModel { get; }
}
