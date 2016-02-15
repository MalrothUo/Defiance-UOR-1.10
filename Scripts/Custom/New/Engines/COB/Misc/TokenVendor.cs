using System; 
using System.Collections.Generic; 
using Server; 

namespace Server.Mobiles 
{ 
   public class TDealer : BaseVendor 
   { 
      private List<SBInfo> m_SBInfos = new List<SBInfo>(); 
      protected override List<SBInfo> SBInfos{ get { return m_SBInfos; } } 

      [Constructable] 
      public TDealer() : base( "the Token Reward Dealer" ) 
      { 
         SetSkill( SkillName.Tailoring, 60.0, 83.0 ); 
      } 

      public override void InitSBInfo() 
      { 
         m_SBInfos.Add( new SBTDealer() ); 
      } 

      public override VendorShoeType ShoeType 
      { 
         get{ return Utility.RandomBool() ? VendorShoeType.Sandals : VendorShoeType.Shoes; } 
      } 

      public TDealer( Serial serial ) : base( serial ) 
      { 
      } 

      public override void Serialize( GenericWriter writer ) 
      { 
         base.Serialize( writer ); 

         writer.Write( (int) 0 ); // version 
      } 

      public override void Deserialize( GenericReader reader ) 
      { 
         base.Deserialize( reader ); 

         int version = reader.ReadInt(); 
      } 
   } 
}