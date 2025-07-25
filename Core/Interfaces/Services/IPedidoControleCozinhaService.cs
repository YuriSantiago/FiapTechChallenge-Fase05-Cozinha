﻿using Core.DTOs;
using Core.Enums;
using Core.Requests.Update;

namespace Core.Interfaces.Services
{
    public interface IPedidoControleCozinhaService
    {

        IList<PedidoControleCozinhaDTO> GetAll();

        IList<PedidoControleCozinhaDTO> GetAllByStatus(StatusPedido statusPedido);

        void AtualizarStatusPedido(PedidoUpdateStatusRequest pedidoUpdateStatusRequest);


    }
}
