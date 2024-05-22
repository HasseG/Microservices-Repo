using AutoMapper;
using Grpc.Core;
using PlatformService.Data;

namespace PlatformService.SyncDataService.Grpc;

public class GrpcPlatformService : GrpcPlatform.GrpcPlatformBase
{
    private readonly IPlatformRepo _repo;
    private readonly IMapper _mapper;

    public GrpcPlatformService(IPlatformRepo repo, IMapper mapper)
    {
        _repo = repo;
        _mapper = mapper;
    }


    public override Task<PlatformResponse> GetAllPlatforms(GetAllRequest request, ServerCallContext context)
    {
        var response = new PlatformResponse();
        var platforms = _repo.GetAllPlatforms();

        foreach(var p in platforms)
        {
            response.Platform.Add(_mapper.Map<GrpcPlatformModel>(p));
        }
        return Task.FromResult(response);
    }




}