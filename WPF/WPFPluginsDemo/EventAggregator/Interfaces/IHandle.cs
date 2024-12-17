namespace EventAggregator.Interfaces;

public interface IHandle<T>
{
  void Handle(T message);
}