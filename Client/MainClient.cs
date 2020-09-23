using CitizenFX.Core;
using Client.World;
using System.Threading.Tasks;

namespace Client
{
    public class MainClient : BaseScript
    {
        public MainClient()
        {
            WeaponPickups.CreatePickups();
            //Tick += OnTick;
        }

        public async Task OnTick()
        {

        }
    }
}
