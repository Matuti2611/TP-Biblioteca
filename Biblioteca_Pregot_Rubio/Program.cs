using Biblioteca_Pregot_Rubio.Data;
using System;

namespace Biblioteca_Pregot_Rubio
{
    public class Program
    {
        public static void Main(string[] args)
        {
            using var context = new BibliotecaContext();
            var gestor = new GestorBiblioteca(context);
            var ui = new BibliotecaUI(gestor);
            ui.iniciar();
        }
    }
}