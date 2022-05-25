using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InRetailDAL.Dtos
{
    public class ItemNameDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class ItemNameDtoResponse
    {
        public string ErrorMessage { get; set; }
        public List<ItemNameDto> ItemNameDto { get; set; }
    }
}
