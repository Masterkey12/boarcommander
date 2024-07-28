using BarCommander.Core.Models.DTOs;
using Microsoft.AspNetCore.Mvc;
using Service.BoardCommander.DataBuilder;
using Service.BoardCommander.TcpCommunication;
using System.Drawing;

namespace BardCommander.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Produces("application/json")]
    public class BoardController : ControllerBase
    {
        private readonly ILogger<BoardController> _logger;
        private readonly ITcpCommunication _tcpCommunication;
        private readonly IDataBuilderService _dataBuilderService;

        public BoardController(ILogger<BoardController> logger, IDataBuilderService dataBuilderService, ITcpCommunication tcpCommunication)
        {
            _logger = logger;
            _tcpCommunication = tcpCommunication;
            _dataBuilderService = dataBuilderService;
        }

        /// <summary>
        /// Cette route permet d'envoyer des instructions � un panneau d'affichage pr�cis
        /// </summary>
        /// <param name="boardKey">Identifiant du panneau d'affichage dans le fichier appsettings.json</param>
        /// <param name="boardCommandDTO">Est la donn�e transmise dans la route</param>
        /// <returns>Une confirmation ou une exception</returns>
        /// 
        /// <remarks>
        ///     Sample request:
        ///
        ///     POST /Board1
        ///     {
        ///        "color": "R",
        ///        "value": "200",
        ///        "position": 1
        ///     }
        ///
        /// </remarks>
        /// <response code="200">Commande transf�r�e avec succ�s</response>
        /// <response code="400">Une erreur li�e � la requ�te ou aux configurations</response>
        /// <response code="500">Une erreur li�e � l'application, non g�r�e</response>
        [HttpPost("{boardKey}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Set(string boardKey, [FromBody] BoardCommandDTO boardCommandDTO)
        {
            var color = _dataBuilderService.RegisterValue(18, "2", boardCommandDTO.Value, boardCommandDTO.Position + 1);
            var value = _dataBuilderService.RegisterColor(16, "2", boardCommandDTO.Color, boardCommandDTO.Position + 2);
            var date = _dataBuilderService.RegisterClock("2", 8);
            var date1 = _dataBuilderService.RegisterClock("3", 2);

            // Send Date and Data
            await Task.WhenAll(_tcpCommunication.SendDataWithAsync(boardKey, value),
                                _tcpCommunication.SendDataWithAsync(boardKey, color),
                                _tcpCommunication.SendDataWithAsync(boardKey, date),
                                _tcpCommunication.SendDataWithAsync(boardKey, date1));
            await Task.Delay(1500);

            return Ok("done");
        }

        /// <summary>
        /// Cette route permet d'envoyer des instructions multiples � un panneau d'affichage pr�cis
        /// </summary>
        /// <param name="boardKey">Identifiant du panneau d'affichage dans le fichier appsettings.json</param>
        /// <param name="boardCommandDTOs">Est la liste de donn�es transmise dans la route</param>
        /// <returns>Une confirmation ou une exception</returns>
        /// 
        /// <remarks>
        ///     Sample request:
        ///
        ///     POST /Board1/SetArray
        ///     {
        ///        "color": "R",
        ///        "value": "200",
        ///        "position": 1
        ///     }
        ///
        /// </remarks>
        /// <response code="200">Commandes transf�r�es avec succ�s</response>
        /// <response code="400">Une erreur li�e � la requ�te ou aux configurations</response>
        /// <response code="500">Une erreur li�e � l'application, non g�r�e</response>
        [HttpPost("{boardKey}/SetArray")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> SetArray(string boardKey, [FromBody] IEnumerable<BoardCommandDTO> boardCommandDTOs)
        {
            // Send Current date
            var date = _dataBuilderService.RegisterClock("2", 8);
            var date1 = _dataBuilderService.RegisterClock("3", 2);
            await Task.WhenAll(_tcpCommunication.SendDataWithAsync(boardKey, date),
                                _tcpCommunication.SendDataWithAsync(boardKey, date1));

            // Send Data
            foreach (var boardCommandDTO in boardCommandDTOs)
            {
                var color = _dataBuilderService.RegisterValue(18, "2", boardCommandDTO.Value, boardCommandDTO.Position + 1);
                var value = _dataBuilderService.RegisterColor(16, "2", boardCommandDTO.Color, boardCommandDTO.Position + 2);

                await Task.WhenAll(_tcpCommunication.SendDataWithAsync(boardKey, value),
                                    _tcpCommunication.SendDataWithAsync(boardKey, color));

            }
            return Ok("done");
        }
    }
}
