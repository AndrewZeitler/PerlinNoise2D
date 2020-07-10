namespace Utils {

    public class Range {
        public float min;
        public float max;

        public Range(float min, float max) { this.min = min; this.max = max; }
        public bool IsInRange(double val) { return (val >= min && val < max); }
        public double GetDeviance(double val) {
            if(!IsInRange(val)) return 0;
            double result = val - min;
            if(result * 2 > max - min) result -= (max - min) / 2;
            return 2 * result / (max - min);
        }
    }

    public class RangeInt {
        public int min;
        public int max;

        public RangeInt(int min, int max) { this.min = min; this.max = max; }
        public bool IsInRange(double val) { return (val >= min && val < max); }
        public double GetDeviance(double val) {
            if(!IsInRange(val)) return 0;
            double result = val - min;
            if(result * 2 > max - min) result -= (max - min) / 2f;
            return 2 * result / (max - min);
        }
    }

}