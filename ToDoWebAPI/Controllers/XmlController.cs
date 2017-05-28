using System.Linq;
using System.Net;
using System.Web.Http;
using System.Web.Http.Description;
using Services;
using ToDoDAL.Abstract;
using ToDoDAL.Model;
using System.Threading.Tasks;

namespace ToDoWebAPI.Controllers
{
    [RoutePrefix("api/xml")]
    public class XmlController : ApiController
    {
        private IRepository<Group> _repository;

        public XmlController(IRepository<Group> repository)
        {
            _repository = repository;
        }

        [HttpPost]        
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> CreateGroupXml()
        {
            var groupXmlCreator = new GroupXmlConverter();
            var categories = (await _repository.GetListAsync()).ToList();

            var elem = groupXmlCreator.CreateXElement(categories);
            groupXmlCreator.SaveGroupXml(elem);
            return StatusCode(HttpStatusCode.OK);
        }
    }
}
