using ChatAppServer.Util;
 

namespace ChatAppServer.Model
{
    public class SignalTargetDataModel
    {
        public string TargetId { get; set; }
        public string SenderId { get; set; }
        public string SenderName {  get; set; }
        public string SenderImgUrl {  get; set; }

        public string WebRtcData {  get; set; }
        public bool IsVideoCall { get; set; }
        private string _dataModelTypeEnum;
        /// <summary>
        /// DataModelType Str
        /// </summary>
        public string DataModelTypeEnum
        {
            get => _dataModelTypeEnum; set
            {

                Console.WriteLine($"\nData Model Command Updated from -> {_dataModelTypeEnum} to -> {value}");
                _dataModelTypeEnum = value;
             
             }
        }

       
        public DataModelTypeEnum GetDataModelTypeEnum()
        {
            return EnumUtil.GetEnumFromString<DataModelTypeEnum>(DataModelTypeEnum);
        }
        public void SetDataModelTypeEnum(DataModelTypeEnum dataModelTypeEnum)
        {
            _dataModelTypeEnum = EnumUtil.GetStrFromEnum(dataModelTypeEnum);
        }
    }
    
    public enum DataModelTypeEnum
    {
        OFFER,
        ANSWER,
        ICE_CANDIDATE,
        /// <summary>
        /// For the call reseiver
        /// </summary>
        INCOMING_CALL,
        NONE,
        /// <summary>
        /// For the caller
        /// </summary>
        CALL_ACCEPTED,
        /// <summary>
        /// For the caller and the receiver 
        /// </summary>
        CALL_REJECTED_OR_END,
        /// <summary>
        /// For the caller
        /// </summary>
        VIDEO_CALL_ACCEPTED,
        /// <summary>
        /// For the caller
        /// </summary>
        CAMERA_OFF,
        /// <summary>
        /// For the caller
        /// </summary>
        CAMERA_ON,
        CALL_TIMEOUT,
        /// <summary>
        /// For the caller
        /// </summary>
        CALL_RECEIVER_BUSY,
        /// <summary>
        /// For the call reseiver
        /// </summary>
        ANOTHER_INCOMING_CALL,
        /// <summary>
        /// For the caller
        /// </summary>
        OUTGOING_CALL,
        /// <summary>
        /// For the call reseiver
        /// </summary>
        CONFIRM_CALL,
        /// <summary>
        /// For the call reseiver
        /// </summary>
        CALL_CONFIRM_SUCCESS,
        /// <summary>
        /// For the call reseiver
        /// </summary>
        CALL_CONFIRM_UNSUCCESSFUL
    }   
     

}
