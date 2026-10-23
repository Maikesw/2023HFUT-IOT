public class code2 {
    public static class Point2D {
        protected int x, y;
    
        public Point2D(int x, int y) {
            this.x = x;
            this.y = y;
        }
    
        public void offset(int a, int b) {
            this.x += a;
            this.y += b;
        }
    
        public double distanceTo(Point2D other) {
            return Math.sqrt(Math.pow(this.x - other.x, 2) + Math.pow(this.y - other.y, 2));
        }
    }
    public static class Point3D extends Point2D {
        private int z;
    
        public Point3D(int x, int y, int z) {
            super(x, y); 
            this.z = z;
        }
    
        public Point3D(Point2D p, int z) {
            super(p.x, p.y);
            this.z = z;
        }
    
        public void offset(int a, int b, int c) {
            super.offset(a, b); 
            this.z += c;
        }

        public double distanceTo(Point3D other) {
            return Math.sqrt(Math.pow(this.x - other.x, 2) + Math.pow(this.y - other.y, 2) + Math.pow(this.z - other.z, 2));
        }

    }
    public static void main(String[] args) {
        Point2D p2d1 = new Point2D(1, 2);
        Point2D p2d2 = new Point2D(4, 6);

        System.out.println( p2d1.distanceTo(p2d2));

        Point3D p3d1 = new Point3D(1, 2, 3);
        Point3D p3d2 = new Point3D(4, 6, 8);

        System.out.println( p3d1.distanceTo(p3d2));
    }
}
