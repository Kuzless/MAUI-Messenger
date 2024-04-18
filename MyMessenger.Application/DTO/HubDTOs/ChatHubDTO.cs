using MyMessenger.Application.DTO.MessagesDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyMessenger.Application.DTO.HubDTOs
{
    public class ChatHubDTO
    {
        public MessageDTO Message { get; set; }
        public AllDataRetrievalParametersDTO Parameters { get; set; }
    }
}
