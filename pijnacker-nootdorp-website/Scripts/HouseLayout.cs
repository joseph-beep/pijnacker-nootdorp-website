public class HouseLayout
{
    public enum RoomType { BEDROOM, BATHROOM, LIVING_ROOM, KITCHEN, TOILET, WORKSPACE, DINING_ROOM }

    public class Room
    {
        public RoomType type;
        public int amount;
    }

    public class Storey
    {
        public int level;
        public Room[] rooms;
    }

    public Storey[] storeys;

    public HouseLayout()
    {
        storeys = new Storey[0];
    }
}