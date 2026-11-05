using Bl.Dtos.Position;
using Dal.Models;

namespace Bl.Mapping
{
    internal static class PositionMapper
    {
        public static PositionDto ToDto(Position position)
        {
            return new PositionDto(position.PositionCode, position.PositionName);
        }

        public static Position ToEntity(CreatePositionDto position)
        {
            return new Position() { PositionName = position.PositionName };
        }

        public static Position ToEntity(UpdatePositionDto position)
        {
            return new Position()
            {
                PositionCode = position.PositionCode,
                PositionName = position.PositionName
            };
        }
    }
}
