// <auto-generated>
//     Generated by the protocol buffer compiler.  DO NOT EDIT!
//     source: matchmaking_service.proto
// </auto-generated>
#pragma warning disable 0414, 1591
#region Designer generated code

using grpc = global::Grpc.Core;

namespace SpinnerKing.Interop {
  public static partial class MatchmakingService
  {
    static readonly string __ServiceName = "com.projectx.interop.MatchmakingService";

    static readonly grpc::Marshaller<global::SpinnerKing.Interop.MatchmakeQuery> __Marshaller_com_projectx_interop_MatchmakeQuery = grpc::Marshallers.Create((arg) => global::Google.Protobuf.MessageExtensions.ToByteArray(arg), global::SpinnerKing.Interop.MatchmakeQuery.Parser.ParseFrom);
    static readonly grpc::Marshaller<global::SpinnerKing.Interop.MatchmakeResult> __Marshaller_com_projectx_interop_MatchmakeResult = grpc::Marshallers.Create((arg) => global::Google.Protobuf.MessageExtensions.ToByteArray(arg), global::SpinnerKing.Interop.MatchmakeResult.Parser.ParseFrom);

    static readonly grpc::Method<global::SpinnerKing.Interop.MatchmakeQuery, global::SpinnerKing.Interop.MatchmakeResult> __Method_Matchmake = new grpc::Method<global::SpinnerKing.Interop.MatchmakeQuery, global::SpinnerKing.Interop.MatchmakeResult>(
        grpc::MethodType.ServerStreaming,
        __ServiceName,
        "Matchmake",
        __Marshaller_com_projectx_interop_MatchmakeQuery,
        __Marshaller_com_projectx_interop_MatchmakeResult);

    /// <summary>Service descriptor</summary>
    public static global::Google.Protobuf.Reflection.ServiceDescriptor Descriptor
    {
      get { return global::SpinnerKing.Interop.MatchmakingServiceReflection.Descriptor.Services[0]; }
    }

    /// <summary>Base class for server-side implementations of MatchmakingService</summary>
    [grpc::BindServiceMethod(typeof(MatchmakingService), "BindService")]
    public abstract partial class MatchmakingServiceBase
    {
      public virtual global::System.Threading.Tasks.Task Matchmake(global::SpinnerKing.Interop.MatchmakeQuery request, grpc::IServerStreamWriter<global::SpinnerKing.Interop.MatchmakeResult> responseStream, grpc::ServerCallContext context)
      {
        throw new grpc::RpcException(new grpc::Status(grpc::StatusCode.Unimplemented, ""));
      }

    }

    /// <summary>Client for MatchmakingService</summary>
    public partial class MatchmakingServiceClient : grpc::ClientBase<MatchmakingServiceClient>
    {
      /// <summary>Creates a new client for MatchmakingService</summary>
      /// <param name="channel">The channel to use to make remote calls.</param>
      public MatchmakingServiceClient(grpc::ChannelBase channel) : base(channel)
      {
      }
      /// <summary>Creates a new client for MatchmakingService that uses a custom <c>CallInvoker</c>.</summary>
      /// <param name="callInvoker">The callInvoker to use to make remote calls.</param>
      public MatchmakingServiceClient(grpc::CallInvoker callInvoker) : base(callInvoker)
      {
      }
      /// <summary>Protected parameterless constructor to allow creation of test doubles.</summary>
      protected MatchmakingServiceClient() : base()
      {
      }
      /// <summary>Protected constructor to allow creation of configured clients.</summary>
      /// <param name="configuration">The client configuration.</param>
      protected MatchmakingServiceClient(ClientBaseConfiguration configuration) : base(configuration)
      {
      }

      public virtual grpc::AsyncServerStreamingCall<global::SpinnerKing.Interop.MatchmakeResult> Matchmake(global::SpinnerKing.Interop.MatchmakeQuery request, grpc::Metadata headers = null, global::System.DateTime? deadline = null, global::System.Threading.CancellationToken cancellationToken = default(global::System.Threading.CancellationToken))
      {
        return Matchmake(request, new grpc::CallOptions(headers, deadline, cancellationToken));
      }
      public virtual grpc::AsyncServerStreamingCall<global::SpinnerKing.Interop.MatchmakeResult> Matchmake(global::SpinnerKing.Interop.MatchmakeQuery request, grpc::CallOptions options)
      {
        return CallInvoker.AsyncServerStreamingCall(__Method_Matchmake, null, options, request);
      }
      /// <summary>Creates a new instance of client from given <c>ClientBaseConfiguration</c>.</summary>
      protected override MatchmakingServiceClient NewInstance(ClientBaseConfiguration configuration)
      {
        return new MatchmakingServiceClient(configuration);
      }
    }

    /// <summary>Creates service definition that can be registered with a server</summary>
    /// <param name="serviceImpl">An object implementing the server-side handling logic.</param>
    public static grpc::ServerServiceDefinition BindService(MatchmakingServiceBase serviceImpl)
    {
      return grpc::ServerServiceDefinition.CreateBuilder()
          .AddMethod(__Method_Matchmake, serviceImpl.Matchmake).Build();
    }

    /// <summary>Register service method with a service binder with or without implementation. Useful when customizing the  service binding logic.
    /// Note: this method is part of an experimental API that can change or be removed without any prior notice.</summary>
    /// <param name="serviceBinder">Service methods will be bound by calling <c>AddMethod</c> on this object.</param>
    /// <param name="serviceImpl">An object implementing the server-side handling logic.</param>
    public static void BindService(grpc::ServiceBinderBase serviceBinder, MatchmakingServiceBase serviceImpl)
    {
      serviceBinder.AddMethod(__Method_Matchmake, serviceImpl == null ? null : new grpc::ServerStreamingServerMethod<global::SpinnerKing.Interop.MatchmakeQuery, global::SpinnerKing.Interop.MatchmakeResult>(serviceImpl.Matchmake));
    }

  }
}
#endregion
