using ChatAppServer.Util;
 

namespace ChatAppServer.Model
{
    public class DataModel
    {
        public string TargetId { get; set; }
        public string SenderId { get; set; }
        public string SenderName {  get; set; }
        public string SenderImgUrl {  get; set; }

        public string WebRtcData {  get; set; }
        public bool IsVideoCall { get; set; }
        private string _dataModelTypeEnum;
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
         
    }
    
    public enum DataModelTypeEnum
    {
        OFFER,
        ANSWER,
        ICE_CANDIDATE,
        INCOMING_CALL,
        NONE,
        CALL_ACCEPTED,
        CALL_REJECTED_OR_END,
        VIDEO_CALL_ACCEPTED,
        CAMERA_OFF,
        CAMERA_ON,
        CALL_TIMEOUT,
        CALL_RECEIVER_BUSY,
        SECOND_INCOMING_CALL
    }   
     

}
