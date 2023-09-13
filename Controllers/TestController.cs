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
    }

    [HttpGet(Name = "GetTest")]
    public string Get()
    {
        return "{\"noms\":[\"Adam\", \"Amelie\", \"Shawn\", \"Antoine\"],\"no_espece\":1,\"nom_espece\":\"Chien\",\"animaux\":[{\"no\":1,\"nom\":{},\"age\":42},{\"no\":2,\"nom\":{},\"age\":40},{\"no\":3,\"nom\":\"Camomille\",\"age\":41},{\"no\":4,\"nom\":\"Blue\",\"age\":39},{\"no\":5,\"nom\":{},\"age\":40},{\"no\":6,\"nom\":\"Norbert\",\"age\":41},{\"no\":20,\"nom\":\"C\u00E9leste\",\"age\":42},{\"no\":21,\"nom\":\"Silhouette\",\"age\":42},{\"no\":36,\"nom\":\"Raven\",\"age\":40},{\"no\":37,\"nom\":\"Brownie\",\"age\":42},{\"no\":38,\"nom\":\"Blacky\",\"age\":39},{\"no\":39,\"nom\":\"Slate\",\"age\":40},{\"no\":53,\"nom\":\"Rex\",\"age\":42},{\"no\":54,\"nom\":\"Coffee\",\"age\":40},{\"no\":55,\"nom\":\"Cocoa\",\"age\":41},{\"no\":56,\"nom\":\"Cola\",\"age\":41},{\"no\":57,\"nom\":\"Peluche\",\"age\":40},{\"no\":58,\"nom\":{},\"age\":41},{\"no\":59,\"nom\":\"Yin-Yang\",\"age\":39},{\"no\":60,\"nom\":\"Lichette\",\"age\":42},{\"no\":61,\"nom\":\"Dice\",\"age\":41},{\"no\":62,\"nom\":{},\"age\":42}]}";
    }
}
