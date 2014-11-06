using System;

namespace Whidsoft.WebControls
{
	/// <summary>
	/// IOrgChart ��ժҪ˵����
	/// </summary>
	public interface IOrgNode
	{
        string ID
        {
            set;
            get;
        }
        string Text
        {
            set;
            get;
        }

        IOrgNode Parent
        {
            set;
            get;
        }

        string Type
        {
            set;
            get;
        }



	}


}
