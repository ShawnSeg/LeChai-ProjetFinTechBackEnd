using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using QueryBuilderTools;
using SQLExecutorTools;

namespace LeChai_ProjetFinTechBackEnd.Controllers;

[ApiController]
[Route("test")]
public class Controller : ControllerBase
{
    private static string connectionString = null;
    private SelectQuery query;
    private SelectQuery queryMain;
    private SQLExecutor executor;
    private CancellationToken cancellationToken = default;
    private readonly ILogger<Controller> _logger;
    private static void setConnectionString()
    {
        if (connectionString is not null)
            return;
        
        IConfiguration configuration = new ConfigurationBuilder()
        .SetBasePath(Directory.GetCurrentDirectory())
        .AddJsonFile("appsettings.json")
        .Build();

        connectionString = configuration.GetConnectionString("DefaultConnection") ?? "error";
    }

    public Controller(ILogger<Controller> logger)
    {
        setConnectionString();
        _logger = logger;
        Table genres = new Table("T_Genres",
            new Column("no", DataType.INT, true),
            new Column("nom", DataType.VARCHAR)
        );
        Table races = new Table("T_Races",
            new Column("no", DataType.INT, true),
            new Column("nom", DataType.VARCHAR),
            new Column("commentaire", DataType.VARCHAR)
        );
        Table especes = new Table("T_Especes",
            new Column("no", DataType.INT, true),
            new Column("nom", DataType.VARCHAR),
            new Column("no_genre", genres)
        );
        Table animaux = new Table("T_Animaux",
            new Column("no", DataType.INT, true),
            new Column("sexe", DataType.VARCHAR),
            new Column("date_naissance", DataType.DATETIME),
            new Column("nom", DataType.VARCHAR),
            new Column("commentaire", DataType.VARCHAR),
            new Column("no_espece", especes),
            new Column("no_race", races)
        );
        animaux
            .addColumn(new Column("no_pere", animaux))
            .addColumn(new Column("no_mere", animaux))
            ;
        Variable var = new Variable(DataType.VARCHAR, '%');
        Variable nom = new Variable("nom", DataType.VARCHAR, true);
        Variable id_espece = new Variable("id_espece", DataType.INT);
        query = new QueryBuilder(DataType.table)
            .select(animaux["no"].noAlias, animaux["nom"].noAlias, Functions.getDateDiff(animaux["date_naissance"], "MONTH").addAlias("age"))
            .from(animaux)
            .where(
                //new Condition(animaux["nom"], new ParsableValue(DataType.VARCHAR, var.addOperator(OperatorsArithm.ADDITION), nom.addOperator(OperatorsArithm.ADDITION), var.addOperator(OperatorsArithm.ADDITION)), ConditionalOperators.LIKE),
                new Condition(animaux["no_espe" +
                "ce"], id_espece, ConditionalOperators.EQUALS))
            .order(animaux["no"].isAsc)
            .BuildSelect(false)
            ;

        queryMain = new QueryBuilder(DataType.table)
            .select(especes["no"].addAlias("no_espece"), especes["nom"].addAlias("nom_espece"))
            .from(especes)
            .BuildSelect(false)
            ;
            
        executor = new SQLExecutor(connectionString);
    }

    [HttpGet("GetArray")]
    public async Task<IActionResult> Get()
    {
        VariableList vars = new VariableList()
                .Add("nom", "a", DataType.VARCHAR)
                ;
        return Ok(await executor.getJSONArray("SELECT no, nom, 1 AS age FROM T_Animaux", new List<Field>() {
                    new Field("no"),
                    new Field("nom"),
                    new Field("age")
            }, vars));
    }

    [HttpGet("GetObject")]
    public async Task<IActionResult> GetObj()
    {
        VariableList vars = new VariableList()
                .Add("nom", "a", DataType.VARCHAR)
                ;
        Dictionary<string, string> binder = new Dictionary<string, string>();
        binder.Add("id_espece", "no_espece");
        return Ok(await executor.getJSONObject(queryMain, new List<Field>() { 
                new Field("no_espece"),
                new Field("nom_espece"),
                new FieldDetailed("animaux", query, null, binder, new List<Field>() {
                    new Field("no"),
                    new Field("nom"),
                    new Field("age")
                })
            }, null, cancellationToken));
    }
}
