namespace UnityCommonLibrary {

    public interface IWorldBounded3D {

        void OnEnteredWorldBounds(WorldBounds3D wb);

        void OnLeftWorldBounds(WorldBounds3D wb);
    }

    public interface IWorldBounded2D {

        void OnEnteredWorldBounds2D(WorldBounds2D wb);

        void OnLeftWorldBounds2D(WorldBounds2D wb);
    }
}