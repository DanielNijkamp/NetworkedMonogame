using MonoGameNetworking.Commands;

namespace MonoGameNetworking.Player;

public interface IPlayerController
{
    public void UpdateMovementData(object sender, ICommand command);
}
