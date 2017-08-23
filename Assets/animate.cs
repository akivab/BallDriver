using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.IO.Ports;
using System.Threading;
using System.Text.RegularExpressions;

public class animate : MonoBehaviour {
	public model ball;

	public int state;
	public int framecounter;
	string transitionName="";
	FullBallEffect effect1,effect2;
	transition trans;
	public int effectCount=0;
	public int maxeffectCount=0;
	public string loaded="";
	string indent="";
	public Vector2[] polar;
	public string[] fulllist;
	public Texture2D[] imageList;
	public Texture2D[] RGBSectionsList;
	FullBallEffect[] active=new FullBallEffect[4];

	SerialPort sp=null;
	public string currentPort=null;
	public string[] ComPorts;
	int promoState=0;
	int promoNumber;
	string promoString="";
	public int specialMode=0;


	static string[] initlist=new string[]{
		"sparkleother",
		"overlay",
		"wander",
		"RGBSections",
		"checker",
		"spiral",
		"divisions",
		"eye",
		"throb",
		"image",
		"ColorWash",
		"halves",
		"rgbLayers",
		"colorSparkle",
		"edgechase",
		"tiling",
		"life",
		"fillers",
		"factions"
	};
	void selectEffect()
	{
		if(specialMode>0)
		{
			effect2=makeEfffect("pumpkin");
			return;
		}
		switch (promoState) {
		case 0:
			effect2 = makeRandomEffectOrSolid (.1f);
			break;
		case 1:
			effect2 = makePromoEfffect ();
			break;
		case 2:
			effect2 = makePromoEfffect ();
			promoState = 3;
			break;
		case 3:
			effect2 = makeRandomEffectOrSolid (.1f);
			promoState = 2;
			break;
		}
	}
	static string[] transitionList=new string[]
	{
		"dissolve",
		"fillOut",
		"crossfade",
		"wipe",
		"triangleReplace"
	};

	public string selectItem(string[] list)
	{
		int i;
		int idx=Random.Range(0,list.Length/2);
		string result=list[idx];
		for(i=idx;i<(list.Length-1);i++)
			list[i]=list[i+1];
		list[i]=result;
		return result;
	}

	string[] readlist(string path)
	{
		TextAsset filedata=Resources.Load<TextAsset>(path);
		string[] lines = Regex.Split(filedata.text,"\n|\r|\n\r");  
		return lines;
	}

	static Texture2D LoadPNG(string filePath) {

		Texture2D tex = null;
		byte[] fileData;

		if (File.Exists(filePath))     {
			fileData = File.ReadAllBytes(filePath);
			tex = new Texture2D(2, 2);
			tex.LoadImage(fileData); //..this will auto-resize the texture dimensions.
		}
		return tex;
	}

	Texture2D[] loadImages(string[]fileNames)
	{
		string path=datapath();
		Texture2D[] textures=new Texture2D[fileNames.Length];
		for(int i=0;i<fileNames.Length;i++)
		{
			textures[i]=LoadPNG(path+"/Resources/"+fileNames[i]);	
		}
		return textures;
	}

	public string selectRandomEfffect()
	{
		if(effectCount>6)
			return "Solid";
		return selectItem(fulllist);
	}

	public FullBallEffect makeEfffect(string name)
	{
		FullBallEffect effect=(FullBallEffect)ScriptableObject.CreateInstance(name);
		effect.master=this;
		loaded+="\n"+indent+name;
		string oldindent=indent;
		indent+="-";
		effect.init();
		indent=oldindent;
		effectCount++;
		if(effectCount>maxeffectCount)
			maxeffectCount=effectCount;
		return effect;
	}
	public  FullBallEffect makeRandomEfffect()
	{
		return makeEfffect(selectRandomEfffect());
	}

	public Color randomColor()
	{
		Color c=new Color(Random.value,Random.value,Random.value);
		return c;
	}
	public FullBallEffect makeRandomEffectOrSolid(float probSolid)
	{
		if(Random.value<probSolid)
			return makeEfffect("Solid");
		return makeRandomEfffect();
	}

	void selectTransition()
	{
		transitionName=selectItem(transitionList);
		//transitionName="dissolve";
		trans=(transition)ScriptableObject.CreateInstance(transitionName);
		trans.master=this;
		trans.init();
	}

	void copyEffect()
	{
		if(effect1!=null)
			effect1.kill();
		effect1=effect2;
		state=0;
		framecounter=160;
		if(loaded=="\nSolid")
			framecounter=80;
	}

	string datapath()
	{
//		if(Application.platform==RuntimePlatform.OSXPlayer)
//			return  Application.dataPath + "../../";
		return Application.dataPath+"/";
	}

	string[] hardcoded={
		"COM3",
		"A=Solid,255,0,0",
		"B=Solid,0,255,0",
		"C=Solid,0,0,255",
		"D=Solid,0,255,255",
		"E=Solid,255,0,255",
		"F=Solid,255,255,0",
		"G=sparkleother,50,Solid,255,0,0",
		"H=sparkleother,50,Solid,0,255,0",
		"I=sparkleother,50,Solid,0,0,255",
		"J=fillers,0,0,0",
		"K=wander,0,0,0",
		"L=divisions,0,0,0",
		"M=colorSparkle,30,0,0",
		"N=factions,0,0,0",
		"O=checker,8,8,0,0,255,Solid,0,0,64,speed +-4 12,count 1 6,r,g,b,effect...",
		"P=spiral,1,-8,6,0,255,255,Solid,0,0,64,twist -1 1,speed +-4 12,count 1 6,r,g,b,effect...",
		"Q=checker,-8,2,255,0,255,Solid,0,0,64,speed +-4 12,count 1 6,r,g,b,effect...",
		"R=spiral,1,8,4,255,255,0,Solid,0,0,64,twist -1 1,speed +-4 12,count 1 6,r,g,b,effect...",
		"S=image,0",
		"T=image,1",
		"U=image,2",
		"V=image,3",
		"W=ColorWash",
		"X=eye",
		"Y=GifAnim"
	};

	string[] interactiveList=new string[25];
	string loadInteractice(string filePath)
	{
		string [] fromfile=hardcoded;//readlist(filePath);
		int i;
		for(i=1;i<fromfile.Length;i++)
		{
			string item=fromfile[i];
			if(item[1]=='=')
			{
				interactiveList[item[0]-'A']=item.Substring(2);
			}
		}
		return fromfile[0];
	}

	// Use this for initialization
	void Start () {
		FullBallEffect.pool=new BufferPool();
//		string path=Application.dataPath;
//		imageList=loadImages(readlist("image.txt"));
//		RGBSectionsList=loadImages(readlist("rgbsections.txt"));
		loadInteractice("patterns.txt");
		fulllist=initlist;
		selectEffect();
		copyEffect();
	}

	void closeCurrentPort()
	{
		if(sp!=null)
		{
			sp.Close();
			sp=null;
			currentPort=null;
		}
	}
	void openNewPort(string portname)
	{
		if(currentPort!=portname)
		{
			StreamWriter sw=new StreamWriter(datapath()+"/Resources/port.txt");
			sw.WriteLine(portname);
			sw.Close();
		}
		currentPort=portname;
		sp=new SerialPort(currentPort,9600);
		sp.ReadTimeout=1;
		sp.Open();

	}

	public string[]portlist; 
	void checkport()
	{
		int x,y;
		portlist=SerialPort.GetPortNames();
		// first time, saved port
		if(ComPorts==null)
		{
			ComPorts=portlist;
			string[] savedportlist=readlist(datapath()+"/Resources/port.txt");
			if(savedportlist.Length==1)
				openNewPort(savedportlist[0]);
			else
				openNewPort(portlist[0]);
			return;
		}
		// if the current port went away, close it
		for(x=0;x<portlist.Length;x++)
			if(portlist[x]==currentPort)
				break;
		if(x==portlist.Length)
			closeCurrentPort();
		// if a new port was added
		if(portlist.Length>ComPorts.Length)
		{
			for(x=0;x<portlist.Length;x++)
			{
				string testport=portlist[x];
				for(y=0;y<ComPorts.Length;y++)
				{
					string oldtestport=ComPorts[y];
					if(oldtestport==testport)
						break;
				}
				// see if we terminated early
				if(y==ComPorts.Length)
				{
					// its new, switch ports
					closeCurrentPort();
					openNewPort(testport);
					ComPorts=portlist;
					return;
				}
			}
		}
		ComPorts=portlist;
	}
	void checkKeyboard()
	{
		for (char c = 'a'; c < 'z'; c++) {
			if (Input.GetKeyDown (c+"")) {
				serialKeyDown ((char)(c-32));
			}
			if (Input.GetKeyUp (c+"")) {
				serialKeyUp ((char)(c-32));
			}
		}
	}
	// Update is called once per frame
	void Update () {
		checkKeyboard ();
//		checkport();
//		ReadSerial();
	}

	int tick=0;
	string errorstring;

	void OnGUI()
	{
		specialMode=GUI.Toggle(new Rect(0,Screen.height-40,80,20),specialMode>0,"Pumpkin")?1:0;
		string report=
			loaded+"\n"+framecounter+
			"\nticks "+tick+
			"\nstate "+state+
			"\nbuilds "+builds+
			"\nbuffercount "+FullBallEffect.pool.BufferCount();
		tick++;
		report += "\n" + promoString;
		report += "\n" + promoState;
		GUI.skin.box.alignment = TextAnchor.UpperLeft;
		GUI.Box(new Rect(0,20,150,500),report);
	}

	public FullBallEffect makeArgEfffect(string name,int i,string[]argv)
	{
		
		FullBallEffect effect=(FullBallEffect)ScriptableObject.CreateInstance(name);
		effect.master=this;
		effect.init(i,argv);
		return effect;
	}

	public FullBallEffect makeGifEffect() {
		GifAnim effect = (GifAnim)ScriptableObject.CreateInstance("GifAnim");
		effect.master = this;
		effect.LoadGIF("pacman");
		return effect;

	}
	public FullBallEffect makePromoEfffect()
	{
		if (Random.value < 0.1)
		{
			return makeGifEffect();
		}
			
		image effect=(image)ScriptableObject.CreateInstance("image");
		effect.master=this;
		effect.promo (promoNumber);
		return effect;
	}

	void checkPromo(char c)
	{
		if (c == 'X') {
			promoString = "";
			return;
		}
		if (promoString == "ABACADA") {
			promoNumber = c - 'A';
			promoState = 1;
			framecounter=0;
			promoString = "";
			return;
		}
		if (promoString == "ABADACA") {
			promoNumber = c - 'A';
			promoState = 2;
			framecounter=0;
			promoString = "";
			return;
		}
		if(promoString.Length<20)
			promoString += c;
		if (promoString == "ABAACAADA") {
			promoState = 0;
			framecounter=0;
			promoString = "";
			return;
		}
	}

	char[]keys=new char[4];
	void serialKeyDown(char c)
	{
		if (c < 'A' || c > 'Y')
			return;
		framecounter=100;
		checkPromo (c);
		if(effect1!=null)
			effect1.kill();
		effect1=null;
		if(effect2!=null)
			effect2.kill();
		effect2=null;
		int i;
		for(i=0;i<active.Length;i++)
		{
			if(active[i]==null)
				break;
		}
		if(i<active.Length)
		{
			FullBallEffect effect;
			if (c == 'Y')
			{
				effect = makeGifEffect();
			}
			else
			{
				string command = interactiveList[c - 'A'];
				string[] argv = command.Split(',');
				effect = makeArgEfffect(argv[0], 1, argv);
			}

			active[i]=effect;
			keys[i]=c;
		}
		state=2;
	}
	void serialKeyUp(char c)
	{
		for(int i=0;i<keys.Length;i++)
		{
			if(keys[i]==c)
			{
				active[i].kill();
				keys[i]='-';
				active[i]=null;
				break;
			}
		}
		framecounter = 0;
	}

	char updown;
	void ReadSerial()
	{
		try
		{
			char value=(char)sp.ReadChar();
			if(value=='+')
				updown=value;
			if(value=='-')
				updown=value;
			if((value>='A')&&(value<='X'))
			{
				if(updown=='+')
					serialKeyDown(value);
				if(updown=='-')
					serialKeyUp(value);
			}
		}
		catch(System.Exception e)
		{
		}
	}


	int builds=0;
	public void buildFrame(Color[] display,Vector3[] points)
	{
		
		try
		{
			switch(state)
			{
			case 0:
				effect1.buildFrame(display,points);
				builds++;
				framecounter--;
				if(framecounter<=0)
				{
					selectTransition();
					effectCount=0;
					loaded="";
					indent="";
					selectEffect();
					state=1;
					framecounter=100;
				}
				break;
			case 1:
				framecounter--;
				if(framecounter<=0)
				{
					copyEffect();
					break;
				}
				bool done=trans.buildFrame(display,effect1,effect2,points);
				builds++;
				if(done)
				{
					trans.kill();
					copyEffect();
					break;
				}
				break;
			case 2:
				framecounter--;
				if(framecounter<=0)
				{
					for(int i=0;i<active.Length;i++)
					{
						if(active[i]!=null)
						{
							active[i].kill();
							active[i]=null;
//							keys[i]='-';
						}
					}
					selectEffect();
					copyEffect();
				}
				else
				{
					for(int i=0;i<1500;i++)
						display[i]=Color.black;
					for(int i=0;i<active.Length;i++)
					{
						if(active[i]!=null)
						{
							active[i].buildFrame(display,points);
							framecounter=100;
						}
					}
				}
				break;
			}
		}
		catch(System.Exception e)
		{
			errorstring=e.ToString();
		}

	}


}
