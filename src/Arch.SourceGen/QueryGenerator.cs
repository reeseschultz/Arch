using Arch.SourceGen.Fundamentals;
using ArchSourceGenerator;

namespace Arch.SourceGen;

[Generator]
public class QueryGenerator : IIncrementalGenerator
{
    private const int MAX_COMPONENTS = 25;

    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        if (!Debugger.IsAttached)
        {
            //Debugger.Launch();
        }

        context.RegisterPostInitializationOutput(initializationContext =>
        {

            var compileTimeStatics = new StringBuilder();
            compileTimeStatics.AppendLine("using System;");
            compileTimeStatics.AppendLine("namespace Arch.Core.Utils;");
            compileTimeStatics.AppendGroups(MAX_COMPONENTS);

            var delegates = new StringBuilder();
            delegates.AppendLine("using System;");
            delegates.AppendLine("namespace Arch.Core;");
            delegates.AppendForEachDelegates(MAX_COMPONENTS);
            delegates.AppendForEachEntityDelegates(MAX_COMPONENTS);

            var interfaces = new StringBuilder();
            interfaces.AppendLine("using System;");
            interfaces.AppendLine("using System.Runtime.CompilerServices;");
            interfaces.AppendLine("namespace Arch.Core;");
            interfaces.AppendInterfaces(MAX_COMPONENTS);
            interfaces.AppendEntityInterfaces(MAX_COMPONENTS);

            var references = new StringBuilder();
            references.AppendLine("using System;");
            references.AppendLine("using System.Runtime.CompilerServices;");
            references.AppendLine("using CommunityToolkit.HighPerformance;");
            references.AppendLine("namespace Arch.Core;");
            references.AppendComponents(MAX_COMPONENTS);
            references.AppendEntityComponents(MAX_COMPONENTS);

            var jobs = new StringBuilder();
            jobs.AppendLine("using System;");
            jobs.AppendLine("using System.Runtime.CompilerServices;");
            jobs.AppendLine("using ArrayExtensions = CommunityToolkit.HighPerformance.ArrayExtensions;");
            jobs.AppendLine("namespace Arch.Core;");
            jobs.AppendForEachJobs(MAX_COMPONENTS);
            jobs.AppendEntityForEachJobs(MAX_COMPONENTS);
            jobs.AppendIForEachJobs(MAX_COMPONENTS);
            jobs.AppendIForEachWithEntityJobs(MAX_COMPONENTS);

            var accessors = new StringBuilder();
            accessors.AppendLine("using System;");
            accessors.AppendLine("using System.Runtime.CompilerServices;");
            accessors.AppendLine("using JobScheduler;");
            accessors.AppendLine("using Arch.Core.Utils;");
            accessors.AppendLine("using System.Diagnostics.Contracts;");
            accessors.AppendLine("using Arch.Core.Extensions;");
            accessors.AppendLine("using Arch.Core.Extensions.Internal;");
            accessors.AppendLine("using System.Diagnostics.CodeAnalysis;");
            accessors.AppendLine("using CommunityToolkit.HighPerformance;");
            accessors.AppendLine("using ArrayExtensions = CommunityToolkit.HighPerformance.ArrayExtensions;");
            accessors.AppendLine("using System.Buffers;");
            accessors.AppendLine(
                $$"""
                namespace Arch.Core{

                    public partial struct Chunk
                    {
                        {{new StringBuilder().AppendChunkIndexes(MAX_COMPONENTS)}}
                        {{new StringBuilder().AppendChunkHases(MAX_COMPONENTS)}}
                        {{new StringBuilder().AppendChunkIndexGets(MAX_COMPONENTS)}}
                        {{new StringBuilder().AppendChunkIndexGetRows(MAX_COMPONENTS)}}
                        {{new StringBuilder().AppendChunkIndexSets(MAX_COMPONENTS)}}

                        {{new StringBuilder().AppendChunkGetArrays(MAX_COMPONENTS)}}
                        {{new StringBuilder().AppendChunkGetSpans(MAX_COMPONENTS)}}
                        {{new StringBuilder().AppendChunkGetFirsts(MAX_COMPONENTS)}}
                    }

                    public partial class Archetype
                    {
                        {{new StringBuilder().AppendArchetypeHases(MAX_COMPONENTS)}}
                        {{new StringBuilder().AppendArchetypeGets(MAX_COMPONENTS)}}
                        {{new StringBuilder().AppendArchetypeSets(MAX_COMPONENTS)}}
                        {{new StringBuilder().AppendArchetypeSetRanges(MAX_COMPONENTS)}}
                    }

                    public partial class World
                    {
                        {{new StringBuilder().AppendCreates(MAX_COMPONENTS)}}
                        {{new StringBuilder().AppendWorldHases(MAX_COMPONENTS)}}
                        {{new StringBuilder().AppendWorldGets(MAX_COMPONENTS)}}
                        {{new StringBuilder().AppendWorldSets(MAX_COMPONENTS)}}
                        {{new StringBuilder().AppendWorldAdds(MAX_COMPONENTS)}}
                        {{new StringBuilder().AppendWorldRemoves(MAX_COMPONENTS)}}

                        {{new StringBuilder().AppendWorldIdHases(MAX_COMPONENTS)}}
                        {{new StringBuilder().AppendWorldIdGets(MAX_COMPONENTS)}}
                        {{new StringBuilder().AppendWorldIdSets(MAX_COMPONENTS)}}
                        {{new StringBuilder().AppendWorldIdAdds(MAX_COMPONENTS)}}
                        {{new StringBuilder().AppendWorldIdRemoves(MAX_COMPONENTS)}}

                        {{new StringBuilder().AppendQueryMethods(MAX_COMPONENTS)}}
                        {{new StringBuilder().AppendEntityQueryMethods(MAX_COMPONENTS)}}
                        {{new StringBuilder().AppendParallelQuerys(MAX_COMPONENTS)}}
                        {{new StringBuilder().AppendParallelEntityQuerys(MAX_COMPONENTS)}}

                        {{new StringBuilder().AppendQueryInterfaceMethods(MAX_COMPONENTS)}}
                        {{new StringBuilder().AppendEntityQueryInterfaceMethods(MAX_COMPONENTS)}}
                        {{new StringBuilder().AppendHpParallelQuerys(MAX_COMPONENTS)}}
                        {{new StringBuilder().AppendHpeParallelQuerys(MAX_COMPONENTS)}}

                        {{new StringBuilder().AppendSetWithQueryDescriptions(MAX_COMPONENTS)}}
                        {{new StringBuilder().AppendAddWithQueryDescriptions(MAX_COMPONENTS)}}
                        {{new StringBuilder().AppendRemoveWithQueryDescriptions(MAX_COMPONENTS)}}
                    }

                    public partial struct QueryDescription
                    {
                        {{new StringBuilder().AppendQueryDescriptionWithAlls(MAX_COMPONENTS)}}
                        {{new StringBuilder().AppendQueryDescriptionWithAnys(MAX_COMPONENTS)}}
                        {{new StringBuilder().AppendQueryDescriptionWithNones(MAX_COMPONENTS)}}
                        {{new StringBuilder().AppendQueryDescriptionWithExclusives(MAX_COMPONENTS)}}
                    }

                }

                namespace Arch.Core.Extensions{

                    public static partial class EntityExtensions
                    {
                    #if !PURE_ECS
                        {{new StringBuilder().AppendEntityHases(MAX_COMPONENTS)}}
                        {{new StringBuilder().AppendEntitySets(MAX_COMPONENTS)}}
                        {{new StringBuilder().AppendEntityGets(MAX_COMPONENTS)}}
                        {{new StringBuilder().AppendEntityAdds(MAX_COMPONENTS)}}
                        {{new StringBuilder().AppendEntityRemoves(MAX_COMPONENTS)}}
                    #endif
                    }

                }
                """
            );

            initializationContext.AddSource("CompileTimeStatics.g.cs",
                CSharpSyntaxTree.ParseText(compileTimeStatics.ToString()).GetRoot().NormalizeWhitespace().ToFullString());

            initializationContext.AddSource("Delegates.g.cs",
                CSharpSyntaxTree.ParseText(delegates.ToString()).GetRoot().NormalizeWhitespace().ToFullString());

            initializationContext.AddSource("Interfaces.g.cs",
                CSharpSyntaxTree.ParseText(interfaces.ToString()).GetRoot().NormalizeWhitespace().ToFullString());

            initializationContext.AddSource("References.g.cs",
                CSharpSyntaxTree.ParseText(references.ToString()).GetRoot().NormalizeWhitespace().ToFullString());

            initializationContext.AddSource("Jobs.g.cs",
                CSharpSyntaxTree.ParseText(jobs.ToString()).GetRoot().NormalizeWhitespace().ToFullString());

            initializationContext.AddSource("Accessors.g.cs",
                CSharpSyntaxTree.ParseText(accessors.ToString()).GetRoot().NormalizeWhitespace().ToFullString());
        });
    }
}
