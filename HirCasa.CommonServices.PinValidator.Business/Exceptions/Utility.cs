using System.Text.RegularExpressions;

namespace HirCasa.CommonServices.PinValidator.Business.Exceptions;

public static class Utility
{
    public static string ParseMessage(Exception e)
    {
        if (e == null)
            return string.Empty;

        Regex regex = new Regex(@"(\w*.\w*:\w* \d*)");
        string[] origenes = regex.Matches(e.StackTrace!).ToArray().Select(x => x.Value).ToArray();

        // Regresamos el arreglo de mensajes de error unidos por la cadena " -> "
        return string.Join(" -> ", origenes);
    }
}
