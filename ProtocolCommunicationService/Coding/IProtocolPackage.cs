﻿using System;
using System.Collections.Generic;
using SHWDTech.IOT.Storage.Communication.Entities;

namespace ProtocolCommunicationService.Coding
{
    public interface IProtocolPackage
    {
        /// <summary>
        /// 是否已完成编解码
        /// </summary>
        bool Finalized { get; }

        /// <summary>
        /// 协议包分解信息
        /// </summary>
        string PackageComponentFactors { get; }

        /// <summary>
        /// 协议包长度
        /// </summary>
        int PackageByteLenth { get; }

        /// <summary>
        /// 协议包所属设备
        /// </summary>
        IClientSource ClientSource { get; set; }

        /// <summary>
        /// 协议接收时间
        /// </summary>
        DateTime ReceiveDateTime { get; set; }

        /// <summary>
        /// 协议编组码完成时间
        /// </summary>
        DateTime FinalizeDateTime { get; set; }

        /// <summary>
        /// 协议指令定义码
        /// </summary>
        byte[] CommandDefinitionCode { get; }

        /// <summary>
        /// 协议包数据记录ID
        /// </summary>
        ProtocolData ProtocolData { get; set; }

        /// <summary>
        /// 所属协议
        /// </summary>
        Protocol Protocol { get; set; }

        /// <summary>
        /// 所属指令
        /// </summary>
        ProtocolCommand Command { get; set; }

        /// <summary>
        /// 数据段总数
        /// </summary>
        int StructureComponentCount { get; }

        /// <summary>
        /// 设备NODEID号
        /// </summary>
        byte[] DeviceNodeId { get; set; }

        /// <summary>
        /// 设备NODEID字符串
        /// </summary>
        string NodeIdString { get; }

        /// <summary>
        /// 是否是心跳包
        /// </summary>
        bool IsHeartBeat { get; }

        /// <summary>
        /// 协议请求码
        /// </summary>
        string RequestCode { get; }

        /// <summary>
        /// 数据组件索引位置
        /// </summary>
        int DataComponentIndex { get; }

        /// <summary>
        /// 数据包处理参数
        /// </summary>
        List<string> DeliverParams { get; }

        /// <summary>
        /// 协议包状态
        /// </summary>
        PackageStatus Status { get; set; }

        /// <summary>
        /// 获取指定名称的数据段
        /// </summary>
        /// <param name="name">数据段名称</param>
        /// <returns>指定名称的数据段</returns>
        IPackageComponent this[string name] { get; set; }

        /// <summary>
        /// 数据段协议包组件
        /// </summary>
        IPackageComponent DataComponent { get; }

        /// <summary>
        /// 协议数据组件字典
        /// </summary>
        Dictionary<string, IPackageComponent> StructureComponents { get; }

        /// <summary>
        /// 解码失败相关错误信息
        /// </summary>
        IEnumerable<string> ErrorMessages { get; }

        /// <summary>
        /// 添加解码错误错误信息
        /// </summary>
        /// <param name="error"></param>
        void AddDecodeError(string error);

        /// <summary>
        /// 添加数据段数据
        /// </summary>
        /// <param name="component"></param>
        void AppendData(IPackageComponent component);

        /// <summary>
        /// 获取指定名称数据的字符串值
        /// </summary>
        /// <param name="dataValueName"></param>
        /// <returns></returns>
        string GetDataValueString(string dataValueName);

        /// <summary>
        /// 获取需要计算的CRC数据段
        /// </summary>
        /// <returns></returns>
        byte[] GetCrcBytes();

        /// <summary>
        /// 设置协议数据信息
        /// </summary>
        void SetupProtocolData();

        /// <summary>
        /// 获取数据包字节流
        /// </summary>
        /// <returns></returns>
        byte[] GetBytes();

        /// <summary>
        /// 完成协议包的编解码
        /// </summary>
        void Finalization();
    }
}
