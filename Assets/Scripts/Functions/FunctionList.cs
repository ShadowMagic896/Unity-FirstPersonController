public class Functions {

    public static double Abs(double x){
        if (x < 0){
            return -x;
        } return x;
    }

    private static void LogInfo(bool[] items, string[] names) {
        string end = "";
        for (int i = 0; i < items.Length; i++) {
            end += (items[i] ? names[i] : "Not-" + names[i]) + ", ";
        }
        Debug.Log(end);
    }




}