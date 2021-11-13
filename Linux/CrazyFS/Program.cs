namespace CrazyFS
{
    public class Program
    {
        public static void Main (string[] args)
        {
            /*using (RedirectFS fs = new RedirectFS ("/mnt/test/source", "/mnt/test/dest")) {
                fs.Start ();
            }*/
            
            using (CrazyFSFileSystem fs = new ("/mnt/test/source", "/mnt/test/dest")) {
                fs.Start ();
            }
        }
    }
}