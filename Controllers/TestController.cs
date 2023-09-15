using Microsoft.AspNetCore.Mvc;

namespace LeChai_ProjetFinTechBackEnd.Controllers;

[ApiController]
[Route("get")]
public class Controller : ControllerBase
{

    private readonly ILogger<Controller> _logger;

    public Controller(ILogger<Controller> logger)
    {
        _logger = logger;
        Table etats_commandes = new Table("etats_commandes",
            new Column("id", DataType.INT, true),
            new Column("nom", DataType.VARCHAR),
            new Column("descriptions", DataType.VARCHAR)
        );
        
        Variable var = new Variable(DataType.VARCHAR, '%');
        Variable nom = new Variable("nom", DataType.VARCHAR, true);
        Variable id_espece = new Variable("id_espece", DataType.INT);
        query = new QueryBuilder(DataType.table)
            .select(etats_commandes["id"].noAlias, etats_commandes["nom"].noAlias, etats_commandes["descriptions"].noAlias)
            .from(etats_commandes)
            .where(
                new Condition(etats_commandes["nom"], new ParsableValue(DataType.VARCHAR, var.addOperator(OperatorsArithm.ADDITION), nom.addOperator(OperatorsArithm.ADDITION), var.addOperator(OperatorsArithm.ADDITION)), ConditionalOperators.LIKE)
                //new Condition(animaux["no_espece"], id_espece, ConditionalOperators.EQUALS)
            )
            .order(etats_commandes["id"].isAsc)
            .BuildSelect(false)
            ;
        /*
        queryMain = new QueryBuilder(DataType.table)
            .select(especes["no"].addAlias("no_espece"), especes["nom"].addAlias("nom_espece"))
            .from(especes)
            .BuildSelect(false)
            ;
            */
        executor = new SQLExecutor(connectionString);
    }

    [HttpGet("GetArray")]
    public async Task<IActionResult> Get()
    {
        VariableList vars = new VariableList()
                .Add("nom", "q", DataType.VARCHAR)
                ;
        return Ok(await executor.getJSONArray(query, new List<Field>() {
                    new Field("id"),
                    new Field("nom"),
                    new Field("descriptions")
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
