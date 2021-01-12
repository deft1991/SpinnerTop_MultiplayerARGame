// <auto-generated>
//     Generated by the protocol buffer compiler.  DO NOT EDIT!
//     source: player_cards_service.proto
// </auto-generated>
#pragma warning disable 0414, 1591
#region Designer generated code

using grpc = global::Grpc.Core;

namespace SpinnerKing.Interop {
  public static partial class PlayerCardsService
  {
    static readonly string __ServiceName = "com.projectx.interop.PlayerCardsService";

    static readonly grpc::Marshaller<global::Google.Protobuf.WellKnownTypes.Empty> __Marshaller_google_protobuf_Empty = grpc::Marshallers.Create((arg) => global::Google.Protobuf.MessageExtensions.ToByteArray(arg), global::Google.Protobuf.WellKnownTypes.Empty.Parser.ParseFrom);
    static readonly grpc::Marshaller<global::SpinnerKing.Interop.CardsArray> __Marshaller_com_projectx_interop_CardsArray = grpc::Marshallers.Create((arg) => global::Google.Protobuf.MessageExtensions.ToByteArray(arg), global::SpinnerKing.Interop.CardsArray.Parser.ParseFrom);

    static readonly grpc::Method<global::Google.Protobuf.WellKnownTypes.Empty, global::SpinnerKing.Interop.CardsArray> __Method_GetCards = new grpc::Method<global::Google.Protobuf.WellKnownTypes.Empty, global::SpinnerKing.Interop.CardsArray>(
        grpc::MethodType.Unary,
        __ServiceName,
        "GetCards",
        __Marshaller_google_protobuf_Empty,
        __Marshaller_com_projectx_interop_CardsArray);

    /// <summary>Service descriptor</summary>
    public static global::Google.Protobuf.Reflection.ServiceDescriptor Descriptor
    {
      get { return global::SpinnerKing.Interop.PlayerCardsServiceReflection.Descriptor.Services[0]; }
    }

    /// <summary>Base class for server-side implementations of PlayerCardsService</summary>
    [grpc::BindServiceMethod(typeof(PlayerCardsService), "BindService")]
    public abstract partial class PlayerCardsServiceBase
    {
      public virtual global::System.Threading.Tasks.Task<global::SpinnerKing.Interop.CardsArray> GetCards(global::Google.Protobuf.WellKnownTypes.Empty request, grpc::ServerCallContext context)
      {
        throw new grpc::RpcException(new grpc::Status(grpc::StatusCode.Unimplemented, ""));
      }

    }

    /// <summary>Client for PlayerCardsService</summary>
    public partial class PlayerCardsServiceClient : grpc::ClientBase<PlayerCardsServiceClient>
    {
      /// <summary>Creates a new client for PlayerCardsService</summary>
      /// <param name="channel">The channel to use to make remote calls.</param>
      public PlayerCardsServiceClient(grpc::ChannelBase channel) : base(channel)
      {
      }
      /// <summary>Creates a new client for PlayerCardsService that uses a custom <c>CallInvoker</c>.</summary>
      /// <param name="callInvoker">The callInvoker to use to make remote calls.</param>
      public PlayerCardsServiceClient(grpc::CallInvoker callInvoker) : base(callInvoker)
      {
      }
      /// <summary>Protected parameterless constructor to allow creation of test doubles.</summary>
      protected PlayerCardsServiceClient() : base()
      {
      }
      /// <summary>Protected constructor to allow creation of configured clients.</summary>
      /// <param name="configuration">The client configuration.</param>
      protected PlayerCardsServiceClient(ClientBaseConfiguration configuration) : base(configuration)
      {
      }

      public virtual global::SpinnerKing.Interop.CardsArray GetCards(global::Google.Protobuf.WellKnownTypes.Empty request, grpc::Metadata headers = null, global::System.DateTime? deadline = null, global::System.Threading.CancellationToken cancellationToken = default(global::System.Threading.CancellationToken))
      {
        return GetCards(request, new grpc::CallOptions(headers, deadline, cancellationToken));
      }
      public virtual global::SpinnerKing.Interop.CardsArray GetCards(global::Google.Protobuf.WellKnownTypes.Empty request, grpc::CallOptions options)
      {
        return CallInvoker.BlockingUnaryCall(__Method_GetCards, null, options, request);
      }
      public virtual grpc::AsyncUnaryCall<global::SpinnerKing.Interop.CardsArray> GetCardsAsync(global::Google.Protobuf.WellKnownTypes.Empty request, grpc::Metadata headers = null, global::System.DateTime? deadline = null, global::System.Threading.CancellationToken cancellationToken = default(global::System.Threading.CancellationToken))
      {
        return GetCardsAsync(request, new grpc::CallOptions(headers, deadline, cancellationToken));
      }
      public virtual grpc::AsyncUnaryCall<global::SpinnerKing.Interop.CardsArray> GetCardsAsync(global::Google.Protobuf.WellKnownTypes.Empty request, grpc::CallOptions options)
      {
        return CallInvoker.AsyncUnaryCall(__Method_GetCards, null, options, request);
      }
      /// <summary>Creates a new instance of client from given <c>ClientBaseConfiguration</c>.</summary>
      protected override PlayerCardsServiceClient NewInstance(ClientBaseConfiguration configuration)
      {
        return new PlayerCardsServiceClient(configuration);
      }
    }

    /// <summary>Creates service definition that can be registered with a server</summary>
    /// <param name="serviceImpl">An object implementing the server-side handling logic.</param>
    public static grpc::ServerServiceDefinition BindService(PlayerCardsServiceBase serviceImpl)
    {
      return grpc::ServerServiceDefinition.CreateBuilder()
          .AddMethod(__Method_GetCards, serviceImpl.GetCards).Build();
    }

    /// <summary>Register service method with a service binder with or without implementation. Useful when customizing the  service binding logic.
    /// Note: this method is part of an experimental API that can change or be removed without any prior notice.</summary>
    /// <param name="serviceBinder">Service methods will be bound by calling <c>AddMethod</c> on this object.</param>
    /// <param name="serviceImpl">An object implementing the server-side handling logic.</param>
    public static void BindService(grpc::ServiceBinderBase serviceBinder, PlayerCardsServiceBase serviceImpl)
    {
      serviceBinder.AddMethod(__Method_GetCards, serviceImpl == null ? null : new grpc::UnaryServerMethod<global::Google.Protobuf.WellKnownTypes.Empty, global::SpinnerKing.Interop.CardsArray>(serviceImpl.GetCards));
    }

  }
}
#endregion
