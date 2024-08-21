namespace Utilities{
    public class StageController{
        public int Stage {get; private set;}= 0;
        public void Next() => Stage++;
        public void Prev() => Stage--;
        public void End() => Stage = -1;
        public void Set(int stage) => Stage = stage;
    }
}