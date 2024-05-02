using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using HirCasa.CommonServices.PinValidator.Business.Domain;

namespace HirCasa.CommonServices.PinValidator.Infrastructure.Persistence.Seeds;

public class ContextDbSeed
{
    public static void Seed(ModelBuilder modelBuilder)
    {
        // // Leemos desde un archivo json los datos iniciales
        // var dataEntityExamples = File.ReadAllText("../HirCasa.CommonServices.PinValidator.Infrastructure/Persistence/Seeds/Data/EntityExamples.json");
        // var entityExamples = JsonSerializer.Deserialize<List<EntityExample>>(dataEntityExamples);

        // // Agregamos los datos iniciales a la base de datos
        // modelBuilder.Entity<EntityExample>().HasData(entityExamples!);
    }
}
