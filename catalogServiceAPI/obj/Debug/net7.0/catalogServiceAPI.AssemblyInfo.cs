//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Reflection;

[assembly: System.Reflection.AssemblyCompanyAttribute("\n          The Featured Bugz\n      ")]
[assembly: System.Reflection.AssemblyConfigurationAttribute("Debug")]
[assembly: System.Reflection.AssemblyDescriptionAttribute(@"
          0.1.0: første version af catalog-service, indholder forbindelse til MongoDB samt metode 'GetAllItems'
          0.1.1: Tilføjet metoden 'GetItemOnID' - Koden er ikke pæn, så den bør ændres
          0.1.2: Ændrede i filer
                 oprettede hjælpeklasse ItemsRepository
                 oprettede interfacet IItemsRepository
                 Ændrede hjælpeklasse ItemsDBContext
                 Slettede interfacet IItemsDBContext
                 -- DB-connection er nede --
          0.1.3: fik funktionalitet fra hjælpeklasserne til at virke
                 Metoder 'GetAllItems' og 'GetItemsOnID' virker nu
                 -- DB-connection er oppe igen
          0.2.0: Metode 'PostItem' virker
                 opdaterede 'GetAllItems' og 'GetItemOnID'
          0.2.1: lavede metoden 'UpdateItem'
                    den virker i Postman med PUT
                    den bruger et ID (int) og en JSON-body
                 alle metoder er testet og virker stadig
          0.3.0: Lavede metoderne 'ScheduledTimer' og 'RemoveExpiredItems' (ikke testet endnu)
                 Tilføjede tekst-doc med data i CSV-format som passer til DB'en
                 
      ")]
[assembly: System.Reflection.AssemblyFileVersionAttribute("\n          0.3.0\n      ")]
[assembly: System.Reflection.AssemblyInformationalVersionAttribute("1.0.0")]
[assembly: System.Reflection.AssemblyProductAttribute("catalogServiceAPI")]
[assembly: System.Reflection.AssemblyTitleAttribute("catalogServiceAPI")]
[assembly: System.Reflection.AssemblyVersionAttribute("1.0.0.0")]

// Generated by the MSBuild WriteCodeFragment class.

