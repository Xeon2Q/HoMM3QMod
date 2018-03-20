using H3QM.Models.Data;

namespace H3QM.Interfaces.Services
{
    public interface ICreatureService
    {
        int Update(CreatureTemplate creature, ref string data);
    }
}