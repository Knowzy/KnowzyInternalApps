﻿// ******************************************************************
// Copyright (c) Microsoft. All rights reserved.
// This code is licensed under the MIT License (MIT).
// THE CODE IS PROVIDED “AS IS”, WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED,
// INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
// IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
// DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT,
// TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH
// THE CODE OR THE USE OR OTHER DEALINGS IN THE CODE.
// ******************************************************************
using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.Knowzy.Domain;

namespace Microsoft.Knowzy.OrdersAPI.Data
{
    public interface IOrdersStore : IDisposable
    {
        Task<bool> Connected();
        IEnumerable<Shipping> GetShippings();
        Shipping GetShipping(string orderId);
        IEnumerable<Receiving> GetReceivings();
        Receiving GetReceiving(string orderId);
        IEnumerable<PostalCarrier> GetPostalCarriers();
        Task UpsertAsync(Domain.Order order);
        Task DeleteOrderAsync(string orderId);
    }
}