using Clasesvideoclub.Data;
using Clasesvideoclub.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using Clasesvideoclub.Repositories;

using (var context = new AplicationDbContext())
{
    context.Database.Migrate();
}

IGenericRepository<Pelicula> repoPelicula = new GenericRepository<Pelicula>();
IGenericRepository<Socio> repoSocio = new GenericRepository<Socio>();
IGenericRepository<Alquiler> repoAlquiler = new GenericRepository<Alquiler>();

int opcion;
do
{
    Console.Clear();
    Console.WriteLine("======================================");
    Console.WriteLine("       SISTEMA GESTIÓN VIDEOCLUB      ");
    Console.WriteLine("======================================");
    Console.WriteLine("1. Registrar Película");
    Console.WriteLine("2. Registrar Socio");
    Console.WriteLine("3. Registrar Alquiler");
    Console.WriteLine("4. Reporte: Alquileres por Socio");
    Console.WriteLine("5. Reporte: Socios con Demora");
    Console.WriteLine("6. Reporte: Películas más Alquiladas");
    Console.WriteLine("7. Reporte: Socio con más Alquileres");
    Console.WriteLine("8. Registrar Devolución (Cobro de Recargo)");
    Console.WriteLine("0. Salir");
    Console.WriteLine();
    Console.Write("Seleccione una opción: ");

    int.TryParse(Console.ReadLine(), out opcion);
    Console.Clear();

    switch (opcion)
    {
        case 1: RegistrarPelicula(); break;
        case 2: RegistrarSocio(); break;
        case 3: RegistrarAlquiler(); break;
        case 4: ReporteAlquilerPorSocio(); break;
        case 5: ReporteSociosConDemora(); break;
        case 6: ReportePeliculasMasAlquiladas(); break;
        case 7: ReporteSocioMasAlquilo(); break;
        case 8: RegistrarDevolucion(); break;
        case 0: Console.WriteLine("Saliendo del sistema..."); break;
        default: Console.WriteLine("Opción no válida."); break;
    }

    if (opcion != 0)
    {
        Console.WriteLine("\nPresione una tecla para continuar...");
        Console.ReadKey();
    }
} while (opcion != 0);

void RegistrarPelicula()
{
    Console.WriteLine("========== REGISTRAR PELÍCULA ==========\n");
    Console.Write("Título: ");
    string titulo = Console.ReadLine();
    Console.Write("Autor/Director: ");
    string autor = Console.ReadLine();
    Console.Write("Cantidad Disponible: ");
    int.TryParse(Console.ReadLine(), out int cant);

    repoPelicula.Agregar(new Pelicula { Titulo = titulo, Autor = autor, CantidadDisponible = cant });
    Console.WriteLine("\nPelícula registrada exitosamente.");
}

void RegistrarSocio()
{
    Console.WriteLine("========== REGISTRAR SOCIO ==========\n");
    Console.Write("Nombre: ");
    string nombre = Console.ReadLine();
    Console.Write("Apellido: ");
    string apellido = Console.ReadLine();
    Console.Write("DNI: ");
    string dni = Console.ReadLine();
    Console.Write("Teléfono: ");
    string tel = Console.ReadLine();

    repoSocio.Agregar(new Socio { Nombre = nombre, Apellido = apellido, Dni = dni, Telefono = tel });
    Console.WriteLine("\nSocio registrado exitosamente.");
}

void RegistrarAlquiler()
{
    Console.WriteLine("========== REGISTRAR ALQUILER ==========\n");
    var socios = repoSocio.ObtenerTodos();
    var peliculas = repoPelicula.ObtenerTodos().Where(p => p.CantidadDisponible > 0).ToList();

    if (!socios.Any() || !peliculas.Any())
    {
        Console.WriteLine("Se requieren socios registrados y películas con stock disponible.");
        return;
    }

    Console.WriteLine("Socios:");
    socios.ForEach(s => Console.WriteLine($"{s.Id} - {s.Nombre} {s.Apellido} (DNI: {s.Dni})"));
    Console.Write("\nSeleccione ID del Socio: ");
    int.TryParse(Console.ReadLine(), out int socioId);
    var socio = socios.FirstOrDefault(s => s.Id == socioId);
    if (socio == null) { Console.WriteLine("Socio no encontrado."); return; }

    Console.Write("Días pactados de alquiler: ");
    int.TryParse(Console.ReadLine(), out int dias);
    Console.Write("Precio base por día ($): ");
    decimal.TryParse(Console.ReadLine(), out decimal precioDia);

    Alquiler alquiler = new Alquiler
    {
        SocioId = socio.Id,
        DiasAlquilado = dias,
        FechaAlquiler = DateTime.Now,
        MontoTotal = dias * precioDia
    };

    string continuar;
    do
    {
        Console.WriteLine("\nPelículas Disponibles:");
        peliculas.Where(p => p.CantidadDisponible > 0).ToList()
                 .ForEach(p => Console.WriteLine($"{p.Id} - {p.Titulo} (Disponibles: {p.CantidadDisponible})"));

        Console.Write("\nSeleccione ID de la Película a agregar: ");
        int.TryParse(Console.ReadLine(), out int peliculaId);
        var pelicula = peliculas.FirstOrDefault(p => p.Id == peliculaId && p.CantidadDisponible > 0);

        if (pelicula != null)
        {
            alquiler.Detalles.Add(new AlquilerDetalle { PeliculaId = pelicula.Id });
            pelicula.CantidadDisponible--;
            repoPelicula.Modificar(pelicula);
            Console.WriteLine($"Película '{pelicula.Titulo}' agregada.");
        }
        else
        {
            Console.WriteLine("Película no válida o sin stock.");
        }

        Console.Write("¿Desea agregar otra película al mismo alquiler? (s/n): ");
        continuar = Console.ReadLine()?.ToLower();
    } while (continuar == "s");

    if (alquiler.Detalles.Any())
    {
        repoAlquiler.Agregar(alquiler);
        Console.WriteLine($"\nAlquiler registrado correctamente. Monto Inicial: ${alquiler.MontoTotal}");
    }
}

void RegistrarDevolucion()
{
    Console.WriteLine("========== REGISTRAR DEVOLUCIÓN ==========\n");
    using var context = new AplicationDbContext();
    var alquileresPendientes = context.Alquileres.Include(a => a.Socio).Where(a => !a.Devuelto).ToList();

    if (!alquileresPendientes.Any())
    {
        Console.WriteLine("No hay alquileres pendientes de devolución.");
        return;
    }

    foreach (var a in alquileresPendientes)
    {
        Console.WriteLine($"ID Alquiler: {a.Id} | Socio: {a.Socio.Nombre} {a.Socio.Apellido} | Fecha Alquiler: {a.FechaAlquiler.ToShortDateString()} | Días: {a.DiasAlquilado}");
    }

    Console.Write("\nIngrese ID del Alquiler a devolver: ");
    int.TryParse(Console.ReadLine(), out int id);

    var alquiler = context.Alquileres.Include(a => a.Detalles).FirstOrDefault(a => a.Id == id && !a.Devuelto);
    if (alquiler == null) { Console.WriteLine("Alquiler no encontrado."); return; }

    DateTime hoy = DateTime.Now;
    DateTime fechaLimite = alquiler.FechaAlquiler.AddDays(alquiler.DiasAlquilado);
    decimal recargo = 0;

    if (hoy > fechaLimite)
    {
        int diasRetraso = (int)(hoy - fechaLimite).TotalDays;
        recargo = alquiler.MontoTotal * (0.10m * diasRetraso);
        Console.WriteLine($"\n¡ATENCIÓN! Devolución con {diasRetraso} día(s) de demora. Recargo del 10% por día: ${recargo}");
    }

    alquiler.Devuelto = true;
    alquiler.FechaDevolucion = hoy;
    alquiler.MontoTotal += recargo;

    foreach (var detalle in alquiler.Detalles)
    {
        var pelicula = context.Peliculas.Find(detalle.PeliculaId);
        if (pelicula != null) pelicula.CantidadDisponible++;
    }

    context.SaveChanges();
    Console.WriteLine($"\nDevolución completada. Monto Final Cobrado: ${alquiler.MontoTotal}");
}

void ReporteAlquilerPorSocio()
{
    Console.WriteLine("========== REPORTE: ALQUILERES POR SOCIO ==========\n");
    using var context = new AplicationDbContext();
    var socios = context.Socios.ToList();

    foreach (var s in socios)
    {
        var alquileresActivos = context.Alquileres
            .Include(a => a.Detalles).ThenInclude(d => d.Pelicula)
            .Where(a => a.SocioId == s.Id && !a.Devuelto).ToList();

        Console.WriteLine($"Socio: {s.Nombre} {s.Apellido} (DNI: {s.Dni})");
        if (alquileresActivos.Any())
        {
            foreach (var a in alquileresActivos)
            {
                foreach (var d in a.Detalles)
                {
                    Console.WriteLine($"   - Película: {d.Pelicula.Titulo}");
                }
            }
        }
        else
        {
            Console.WriteLine("   (Sin películas alquiladas actualmente)");
        }
        Console.WriteLine("--------------------------------------------------");
    }
}

void ReporteSociosConDemora()
{
    Console.WriteLine("========== REPORTE: SOCIOS CON DEMORA ==========\n");
    using var context = new AplicationDbContext();
    var hoy = DateTime.Now;

    var alquileresDemorados = context.Alquileres
        .Include(a => a.Socio)
        .Include(a => a.Detalles).ThenInclude(d => d.Pelicula)
        .Where(a => !a.Devuelto && a.FechaAlquiler.AddDays(a.DiasAlquilado) < hoy)
        .ToList();

    if (!alquileresDemorados.Any())
    {
        Console.WriteLine("No hay socios con demora en la devolución.");
        return;
    }

    foreach (var a in alquileresDemorados)
    {
        int diasDemora = (int)(hoy - a.FechaAlquiler.AddDays(a.DiasAlquilado)).TotalDays;
        Console.WriteLine($"Socio: {a.Socio.Nombre} {a.Socio.Apellido} | Tel: {a.Socio.Telefono}");
        Console.WriteLine($"Días de Demora: {diasDemora} día(s)");
        Console.WriteLine("--------------------------------------------------");
    }
}

void ReportePeliculasMasAlquiladas()
{
    Console.WriteLine("========== REPORTE: PELÍCULAS MÁS ALQUILADAS ==========\n");
    using var context = new AplicationDbContext();

    var ranking = context.AlquilerDetalles
        .GroupBy(d => d.Pelicula.Titulo)
        .Select(g => new { Titulo = g.Key, Cantidad = g.Count() })
        .OrderByDescending(r => r.Cantidad)
        .ToList();

    if (!ranking.Any()) { Console.WriteLine("No hay registros de alquileres."); return; }

    foreach (var item in ranking)
    {
        Console.WriteLine($"Película: {item.Titulo} | Cantidad de alquileres: {item.Cantidad}");
    }
}

void ReporteSocioMasAlquilo()
{
    Console.WriteLine("========== REPORTE: SOCIO QUE MÁS ALQUILÓ ==========\n");
    using var context = new AplicationDbContext();

    var topSocio = context.Alquileres
        .GroupBy(a => a.Socio)
        .Select(g => new { Socio = g.Key, Total = g.Sum(a => a.Detalles.Count) })
        .OrderByDescending(x => x.Total)
        .FirstOrDefault();

    if (topSocio == null)
    {
        Console.WriteLine("No hay registros de alquileres.");
        return;
    }

    Console.WriteLine($"El socio con más películas alquiladas es: {topSocio.Socio.Nombre} {topSocio.Socio.Apellido}");
    Console.WriteLine($"Total de películas alquiladas: {topSocio.Total}");
}