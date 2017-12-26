namespace Math.Geometry
{
    public struct Frustum
    {
        public Plane left;
        public Plane right;
        public Plane bottom;
        public Plane top;
        public Plane near;
        public Plane far;

        public Frustum(Plane[] planes)
        {
            left = planes[0];
            right = planes[1];
            bottom = planes[2];
            top = planes[3];
            near = planes[4];
            far = planes[5];
        }

        public Plane this[int index]
        {
            get
            {
                switch (index)
                {
                    case 0:
                        return left;
                    case 1:
                        return right;
                    case 2:
                        return bottom;
                    case 3:
                        return top;
                    case 4:
                        return near;
                    case 5:
                        return far;
                    default:
                        throw new System.ArgumentOutOfRangeException("index", "Must be in range 0 and 5 (inclusive)");
                }
            }

            set
            {
                switch (index)
                {
                    case 0:
                        left = value;
                        break;
                    case 1:
                        right = value;
                        break;
                    case 2:
                        bottom = value;
                        break;
                    case 3:
                        top = value;
                        break;
                    case 4:
                        near = value;
                        break;
                    case 5:
                        far = value;
                        break;
                    default:
                        throw new System.ArgumentOutOfRangeException("index", "Must be in range 0 and 5 (inclusive)");
                }
            }
        }

        public override string ToString()
        {
            return "left (" + left + "), right (" + right + "), bottom (" + bottom + "), top (" + top + "), near (" + near + "), far (" + far + ")";
        }
    }
}
