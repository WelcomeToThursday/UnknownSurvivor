using System.Reflection;
using JetBrains.Annotations;
using SPTarkov.DI.Annotations;
using SPTarkov.Server.Core.DI;
using SPTarkov.Server.Core.Helpers.Server;
using SPTarkov.Server.Core.Models.Eft.Common.Tables;
using SPTarkov.Server.Core.Models.Spt.Config;
using SPTarkov.Server.Core.Routers;
using SPTarkov.Server.Core.Utils;
using Path = System.IO.Path;

namespace UnknownSurvivor;

[Injectable(TypePriority = OnLoadOrder.TraderRegistration - 1), UsedImplicitly]
public class UnknownSurvivor(
    WTTServerCommonLib.WTTServerCommonLib wttServerCommonLib, 
    ModHelper modHelper, 
    ImageRouter imageRouter,
    TimeUtil timeUtil,
    UnknownSurvivorHelper ush, 
    TraderConfig traderConfig,
    RagfairConfig ragfairConfig
) : IOnLoad
{
    public async Task OnLoadAsync(CancellationToken cancellationToken)
    {
        var pathToMod = modHelper.GetAbsolutePathToModFolder(Assembly.GetExecutingAssembly());
        const bool debugLogging = true;
        
        var assembly = Assembly.GetExecutingAssembly();

        await wttServerCommonLib.CustomItemServiceExtended.CreateCustomItems(assembly);
        await wttServerCommonLib.CustomLootspawnService.CreateCustomLootSpawns(assembly);
        await wttServerCommonLib.CustomQuestZoneService.CreateCustomQuestZones(assembly);
        
        var traderImagePath = Path.Combine(pathToMod, "res/unknownsurvivor.jpg");
        var traderBase = modHelper.GetJsonDataFromFile<TraderBase>(pathToMod, "db/base.json");
        
        
        imageRouter.AddRoute(traderBase.Avatar!.Replace(".jpg", ""), traderImagePath);
        UnknownSurvivorHelper.SetTraderUpdateTime(traderConfig, traderBase, timeUtil.GetHoursAsSeconds(1), timeUtil.GetHoursAsSeconds(2));

        ragfairConfig.Traders.TryAdd(traderBase.Id, true);
        ush.AddTraderWithEmptyAssortToDb(traderBase);
        ush.AddTraderToLocales(traderBase, "Ex-Handler...");
         
        await wttServerCommonLib.CustomQuestService.CreateCustomQuests(assembly);
        
        var assort = modHelper.GetJsonDataFromFile<TraderAssort>(pathToMod, "db/assort.json");
        ush.OverwriteTraderAssort(traderBase.Id, assort);

        if (debugLogging)
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("Survivor is eager to meet you!");
            Console.ResetColor();
        }

        await Task.CompletedTask;
    }
}