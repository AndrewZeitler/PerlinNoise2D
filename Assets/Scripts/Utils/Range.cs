namespace Utils {

    public struct Range {
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

}