using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace com.Sconit.Entity.Quote
{
    [Serializable]
    public class ProductInfoBase
    {
        /// <summary>
        /// Id
        /// </summary>
        public int Id
        {
            get;
            set;
        }
        /// <summary>
        /// CustomerCode
        /// </summary>
        public string CustomerCode
        {
            get;
            set;
        }
        /// <summary>
        /// CustomerName
        /// </summary>
        public string CustomerName
        {
            get;
            set;
        }
        /// <summary>
        /// ProductName
        /// </summary>
        public string ProductName
        {
            get;
            set;
        }
        /// <summary>
        /// ProductNo
        /// </summary>
        public string ProductNo
        {
            get;
            set;
        }
        /// <summary>
        /// VersionNo
        /// </summary>
        public string VersionNo
        {
            get;
            set;
        }
        /// <summary>
        /// PT
        /// </summary>
        public string PT
        {
            get;
            set;
        }
        /// <summary>
        /// PCBNum
        /// </summary>
        public string PCBNum
        {
            get;
            set;
        }
        /// <summary>
        /// AdvisePCBNum
        /// </summary>
        public string AdvisePCBNum
        {
            get;
            set;
        }
        /// <summary>
        /// DoubleSideMount
        /// </summary>
        public bool DoubleSideMount
        {
            get;
            set;
        }
        /// <summary>
        /// ChipBurning
        /// </summary>
        public bool ChipBurning
        {
            get;
            set;
        }
        /// <summary>
        /// BurningNum
        /// </summary>
        public string BurningNum
        {
            get;
            set;
        }
        /// <summary>
        /// LightNum
        /// </summary>
        public string LightNum
        {
            get;
            set;
        }
        /// <summary>
        /// BoardMode
        /// </summary>
        public string BoardMode
        {
            get;
            set;
        }
        /// <summary>
        /// ConnPoint
        /// </summary>
        public string ConnPoint
        {
            get;
            set;
        }
        /// <summary>
        /// DeviceShaping
        /// </summary>
        public bool DeviceShaping
        {
            get;
            set;
        }
        /// <summary>
        /// ShapingType
        /// </summary>
        public string ShapingType
        {
            get;
            set;
        }
        /// <summary>
        /// ShapingSecCount
        /// </summary>
        public string ShapingSecCount
        {
            get;
            set;
        }
        /// <summary>
        /// DeviceCoding
        /// </summary>
        public bool DeviceCoding
        {
            get;
            set;
        }
        /// <summary>
        /// CodingType
        /// </summary>
        public string CodingType
        {
            get;
            set;
        }
        /// <summary>
        /// CodingSecCount
        /// </summary>
        public string CodingSecCount
        {
            get;
            set;
        }
        /// <summary>
        /// FCTTest
        /// </summary>
        public bool FCTTest
        {
            get;
            set;
        }
        /// <summary>
        /// TestSec
        /// </summary>
        public string TestSec
        {
            get;
            set;
        }
        /// <summary>
        /// ProductAssembly
        /// </summary>
        public bool ProductAssembly
        {
            get;
            set;
        }
        /// <summary>
        /// AssemblySec
        /// </summary>
        public string AssemblySec
        {
            get;
            set;
        }
        /// <summary>
        /// FinalAssemblyTest
        /// </summary>
        public bool FinalAssemblyTest
        {
            get;
            set;
        }
        /// <summary>
        /// FinalTestSec
        /// </summary>
        public string FinalTestSec
        {
            get;
            set;
        }
        /// <summary>
        /// SpecialReq
        /// </summary>
        public string SpecialReq
        {
            get;
            set;
        }
        /// <summary>
        /// SurfaceCoating
        /// </summary>
        public bool SurfaceCoating
        {
            get;
            set;
        }
        /// <summary>
        /// MaterialNo
        /// </summary>
        public string MaterialNo
        {
            get;
            set;
        }
        /// <summary>
        /// CoatingAcreage
        /// </summary>
        public string CoatingAcreage
        {
            get;
            set;
        }
        /// <summary>
        /// CoatingSec
        /// </summary>
        public string CoatingSec
        {
            get;
            set;
        }
        /// <summary>
        /// ProductFilling
        /// </summary>
        public bool ProductFilling
        {
            get;
            set;
        }
        /// <summary>
        /// FillingPrice
        /// </summary>
        public decimal? FillingPrice
        {
            get;
            set;
        }
        /// <summary>
        /// PackMode
        /// </summary>
        public string PackMode
        {
            get;
            set;
        }
        /// <summary>
        /// OutBox
        /// </summary>
        public string OutBox
        {
            get;
            set;
        }
        /// <summary>
        /// OutBoxPrice
        /// </summary>
        public decimal? OutBoxPrice
        {
            get;
            set;
        }
        /// <summary>
        /// Plate
        /// </summary>
        public string Plate
        {
            get;
            set;
        }
        /// <summary>
        /// PlateNum
        /// </summary>
        public string PlateNum
        {
            get;
            set;
        }
        /// <summary>
        /// PlatePrice
        /// </summary>
        public decimal? PlatePrice
        {
            get;
            set;
        }
        /// <summary>
        /// Partition
        /// </summary>
        public string Partition
        {
            get;
            set;
        }
        /// <summary>
        /// PartitionNum
        /// </summary>
        public string PartitionNum
        {
            get;
            set;
        }
        /// <summary>
        /// PartitionPrice
        /// </summary>
        public decimal? PartitionPrice
        {
            get;
            set;
        }
        /// <summary>
        /// BubbleBag
        /// </summary>
        public string BubbleBag
        {
            get;
            set;
        }
        /// <summary>
        /// BubbleBagPrice
        /// </summary>
        public decimal? BubbleBagPrice
        {
            get;
            set;
        }
        /// <summary>
        /// Blister
        /// </summary>
        public string Blister
        {
            get;
            set;
        }
        /// <summary>
        /// BlisterNum
        /// </summary>
        public string BlisterNum
        {
            get;
            set;
        }
        /// <summary>
        /// BlisterPrice
        /// </summary>
        public decimal? BlisterPrice
        {
            get;
            set;
        }
        /// <summary>
        /// FCLNum
        /// </summary>
        public string FCLNum
        {
            get;
            set;
        }
        /// <summary>
        /// DeliveryAdd
        /// </summary>
        public string DeliveryAdd
        {
            get;
            set;
        }
        /// <summary>
        /// LogisticsFee
        /// </summary>
        public decimal? LogisticsFee
        {
            get;
            set;
        }
        /// <summary>
        /// LogisticsCost
        /// </summary>
        public decimal? LogisticsCost
        {
            get;
            set;
        }
        /// <summary>
        /// OutBoxResult
        /// </summary>
        public decimal? OutBoxResult
        {
            get;
            set;
        }
        /// <summary>
        /// PlateResult
        /// </summary>
        public decimal? PlateResult
        {
            get;
            set;
        }
        /// <summary>
        /// PartitionResult
        /// </summary>
        public decimal? PartitionResult
        {
            get;
            set;
        }
        /// <summary>
        /// BubbleBagResult
        /// </summary>
        public decimal? BubbleBagResult
        {
            get;
            set;
        }
        /// <summary>
        /// BlisterResult
        /// </summary>
        public decimal? BlisterResult
        {
            get;
            set;
        }
        /// <summary>
        /// PackPrice
        /// </summary>
        public decimal? PackPrice
        {
            get;
            set;
        }
        /// <summary>
        /// PackCost
        /// </summary>
        public decimal? PackCost
        {
            get;
            set;
        }

        public string ProjectId
        {
            get;
            set;
        }

        public DateTime? CreateDate
        {
            get;
            set;
        }

        public string Status
        {
            get;
            set;
        }

        public string Source_
        {
            get;
            set;
        }

        public string Remark1
        {
            get;
            set;
        }

        public string Remark2
        {
            get;
            set;
        }
    }
}
