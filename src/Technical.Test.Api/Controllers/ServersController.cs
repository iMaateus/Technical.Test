using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Technical.Test.Business.Interfaces;
using Technical.Test.Business.Models;
using Technical.Test.Business.Untils;

namespace Technical.Test.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ServersController : MainController
    {
        private readonly IServerService _serverService;
        private readonly IVideoService _videoService;

        public ServersController(IServerService serverService,
            IVideoService videoService,
            INotifier notifier) : base(notifier)
        {
            _serverService = serverService;
            _videoService = videoService;
        }

        [HttpGet]
        public async Task<ActionResult<CustomMessage<List<Server>>>> GetAll()
        {
            return CustomResponse(await _serverService.GetAll());
        }

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<CustomMessage<Server>>> GetById(Guid id)
        {
            return CustomResponse(await _serverService.GetById(id));
        }

        [HttpGet("available/{id:guid}")]
        public async Task<ActionResult<CustomMessage<string>>> IsAvailable(Guid id)
        {
            var server = await _serverService.GetById(id);

            if (server == null)
            {
                return CustomResponse();
            }

            return CustomResponse(await _serverService.IsAvailable(server));
        }

        [HttpPost]
        public async Task<ActionResult<CustomMessage<Server>>> Post(Server server)
        {
            return CustomResponse(await _serverService.Add(server));
        }

        [HttpPut("{id:guid}")]
        public async Task<ActionResult<CustomMessage<Server>>> Put(Guid id, Server server)
        {
            if (id != server.Id)
            {
                NotifyError("Request Id different from body");
                return CustomResponse();
            }

            return CustomResponse(await _serverService.Update(server));
        }

        [HttpDelete("{id:guid}")]
        public async Task<ActionResult<CustomMessage<DeleteResult>>> Delete(Guid id)
        {
            return CustomResponse(await _serverService.Delete(id));
        }

        [HttpGet("{serverId:guid}/videos")]
        public async Task<ActionResult<CustomMessage<List<Video>>>> GetVideos(Guid serverId)
        {
            return CustomResponse(await _videoService.GetAll(serverId));
        }

        [HttpGet("{serverId:guid}/videos/{videoId:guid}")]
        public async Task<ActionResult<CustomMessage<Video>>> GetVideoById(Guid serverId, Guid videoId)
        {
            return CustomResponse(await _videoService.GetById(serverId, videoId));
        }

        [HttpGet("{serverId:guid}/videos/{videoId:guid}/binary")]
        public async Task<ActionResult<CustomMessage<string>>> GetBinaryVideoById(Guid serverId, Guid videoId)
        {
            var video = _videoService.GetById(serverId, videoId);

            if (video == null)
            {
                return CustomResponse();
            }

            return CustomResponse(await _videoService.DownloadBinary(videoId));
        }

        [HttpPost("{serverId:guid}/videos")]
        public async Task<ActionResult<CustomMessage<Video>>> PostVideo(Guid serverId, Video video)
        {
            var server = await _serverService.GetById(serverId);

            if (server == null)
            {
                return CustomResponse();
            }

            await _videoService.UploadBinary(video);

            return CustomResponse(await _videoService.Add(serverId, video));
        }

        [HttpDelete("{serverId:guid}/videos/{videoId:guid}")]
        public async Task<ActionResult<CustomMessage<DeleteResult>>> DeleteVideo(Guid serverId, Guid videoId)
        {
            var deleteResult = await _videoService.Delete(serverId, videoId);

            if (deleteResult.DeletedCount > 0)
            {
                await _videoService.DeleteBinary(videoId);
            }

            return CustomResponse(deleteResult);
        }
    }
}
