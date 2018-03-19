using H3QM.Models.Data;

namespace H3QM.Interfaces.Services
{
    public interface IChangeExeService
    {
        bool ChangeHero(string exePath, string marker, byte[] originalHero, byte[] modifiedHero);
    }
}