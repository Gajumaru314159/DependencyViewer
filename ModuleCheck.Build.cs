// Copyright Epic Games, Inc. All Rights Reserved.

using UnrealBuildTool;
using System.Reflection;
using System.Text;
using System.IO;
using System.Linq;
using System;
using System.Collections.Generic;


public class ModuleCheck : ModuleRules
{
    public ModuleCheck(ReadOnlyTargetRules Target) : base(Target)
    {
        PCHUsage = PCHUsageMode.UseExplicitOrSharedPCHs;

        PublicDependencyModuleNames.AddRange(new string[] { "Core", "CoreUObject", "Engine", "InputCore", "EnhancedInput" });

        PrivateDependencyModuleNames.AddRange(new string[] { });

        List<string> failedModules = new();

        StringBuilder sb = new();

        foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
        {
            // System系のアセンブリは除外
            if (assembly.FullName.StartsWith("System")) continue; 

            // ModuleRules を継承しているすべてのクラスを取得
            var moduleRulesTypes = assembly.GetTypes()
                .Where(t => t.IsClass && !t.IsAbstract && t.IsSubclassOf(typeof(ModuleRules)));


            foreach (var type in moduleRulesTypes)
            {
                // 自分自身は除外
                if (type == GetType()) continue;

                try
                {

                    // コンストラクタを探してインスタンス化
                    var constructor = type.GetConstructor(new[] { typeof(ReadOnlyTargetRules) });
                    if (constructor != null)
                    {
                        var rules = (ModuleRules)constructor.Invoke(new object[] { Target });

                        foreach (var name in rules.PublicDependencyModuleNames)
                        {
                            sb.AppendLine($"{type.Name}-->{name}");
                        }
                        foreach (var name in rules.PrivateDependencyModuleNames)
                        {
                            sb.AppendLine($"{type.Name}-.->{name}");
                        }
                    }
                }
                catch (Exception ex)
                {
                    failedModules.Add(ex.Message);
                }
            }
        }

        File.WriteAllText("C:/ModuleDependency.txt", sb.ToString());

    }
}
