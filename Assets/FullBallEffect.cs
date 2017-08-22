﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ColorBuffer
{
	public Color[] colors=new Color[1500];
	public Color this[int index]
	{
		get
		{
			return colors[index];
		}

		set
		{
			colors[index]=value;
		}
	}
}

public class BufferPool
{
	private static List<ColorBuffer> Pool=new List<ColorBuffer>();
	static int maxcount=0;
	public ColorBuffer GetBuffer()
	{
		if(Pool.Count>0)
		{
			ColorBuffer item=Pool[0];
			Pool.Remove(item);
			return item;
		}
		maxcount++;
		return new ColorBuffer();
	}

	public void ReturnBuffer(ColorBuffer item)
	{
		Pool.Add(item);
	}

	public int BufferCount()
	{
		return maxcount;
	}

}

public class FullBallEffect : ScriptableObject {

	public animate master;
	public static BufferPool pool;

	// return a random color with a probability of it being black
	public Color randomColor(float probBlack)
	{
		if(Random.value<probBlack)
			return Color.black;
		return master.randomColor();
	}

	// make an effect of class "name"
	public FullBallEffect makeEfffect(string name)
	{
		return master.makeEfffect(name);
	}

	// make a random effect that does not include solid
	public  FullBallEffect makeRandomEfffect()
	{
		return master.makeRandomEfffect();
	}

	// make a ramdom effects that has a probablility of being solid
	public FullBallEffect makeRandomEffectOrSolid(float probSolid)
	{
		return master.makeRandomEffectOrSolid(probSolid);
	}

	public virtual void init(int i,string[] argv)
	{
		init();
	}

	public virtual void init()
	{
	}

	public virtual void buildFrame(Color[] display,Vector3[] points)
	{
	}

	public virtual void kill()
	{
	}

	static public int[] neighbors=new int[]
	{
		2,1200,300,2,7,1203,0,3,1,2,301,5,5,308,14,3,4,6,5,12,7,1,6,8,7,10,1204,10,23,1215,8,11,9,10,12,21,6,13,11,12,14,19,4,15,13,14,309,17,17,324,49,15,16,18,17,47,19,13,18,20,19,45,21,11,20,22,21,43,23,9,22,24,23,41,1216,
		27,50,1400,27,32,53,25,28,26,27,1401,30,30,1408,39,28,29,31,30,37,32,26,31,33,32,35,54,35,48,65,33,36,34,35,37,46,31,38,36,37,39,44,29,40,38,39,1409,42,24,42,1424,40,41,43,22,44,42,38,43,45,20,46,44,36,45,47,18,48,46,34,47,49,16,66,48,
		25,52,75,52,57,78,50,53,51,26,55,52,33,64,55,53,54,56,55,62,57,51,56,58,57,60,79,60,73,90,58,61,59,60,62,71,56,63,61,62,64,69,54,65,63,34,67,64,49,224,67,65,66,68,67,222,69,63,68,70,69,220,71,61,70,72,71,218,73,59,72,74,73,216,91,
		50,77,1425,77,82,1428,75,78,76,51,80,77,58,89,80,78,79,81,80,87,82,76,81,83,82,85,1429,85,98,1440,83,86,84,85,87,96,81,88,86,87,89,94,79,90,88,59,92,89,74,124,92,90,91,93,92,122,94,88,93,95,94,120,96,86,95,97,96,118,98,84,97,99,98,116,1441,
		102,250,125,102,107,253,100,103,101,102,126,105,105,133,114,103,104,106,105,112,107,101,106,108,107,110,254,110,123,265,108,111,109,110,112,121,106,113,111,112,114,119,104,115,113,114,134,117,99,117,149,115,116,118,97,119,117,113,118,120,95,121,119,111,120,122,93,123,121,109,122,124,91,266,123,
		100,150,127,103,127,132,125,128,126,127,151,130,130,158,139,128,129,131,130,137,132,126,131,133,104,132,135,115,135,148,133,136,134,135,137,146,131,138,136,137,139,144,129,140,138,139,159,142,142,174,1499,140,141,143,142,1497,144,138,143,145,144,1495,146,136,145,147,146,1493,148,134,147,149,116,148,1491,
		125,275,152,128,152,157,150,153,151,152,276,155,155,283,164,153,154,156,155,162,157,151,156,158,129,157,160,140,160,173,158,161,159,160,162,171,156,163,161,162,164,169,154,165,163,164,284,167,167,299,199,165,166,168,167,197,169,163,168,170,169,195,171,161,170,172,171,193,173,159,172,174,141,173,191,
		177,475,1375,177,182,478,175,178,176,177,1376,180,180,1383,189,178,179,181,180,187,182,176,181,183,182,185,479,185,198,490,183,186,184,185,187,196,181,188,186,187,189,194,179,190,188,189,1384,192,174,192,1399,190,191,193,172,194,192,188,193,195,170,196,194,186,195,197,168,198,196,184,197,199,166,491,198,
		202,325,225,202,207,328,200,203,201,202,226,205,205,233,214,203,204,206,205,212,207,201,206,208,207,210,329,210,223,340,208,211,209,210,212,221,206,213,211,212,214,219,204,215,213,214,234,217,74,217,249,215,216,218,72,219,217,213,218,220,70,221,219,211,220,222,68,223,221,209,222,224,66,341,223,
		200,375,227,203,227,232,225,228,226,227,376,230,230,383,239,228,229,231,230,237,232,226,231,233,204,232,235,215,235,248,233,236,234,235,237,246,231,238,236,237,239,244,229,240,238,239,384,242,242,399,274,240,241,243,242,272,244,238,243,245,244,270,246,236,245,247,246,268,248,234,247,249,216,248,266,
		100,252,275,252,257,278,250,253,251,101,255,252,108,264,255,253,254,256,255,262,257,251,256,258,257,260,279,260,273,290,258,261,259,260,262,271,256,263,261,262,264,269,254,265,263,109,267,264,124,249,267,265,266,268,247,269,267,263,268,270,245,271,269,261,270,272,243,273,271,259,272,274,241,291,273,
		150,250,277,153,277,282,275,278,276,251,280,277,258,289,280,278,279,281,280,287,282,276,281,283,154,282,285,165,285,298,283,286,284,285,287,296,281,288,286,287,289,294,279,290,288,259,292,289,274,449,292,290,291,293,292,447,294,288,293,295,294,445,296,286,295,297,296,443,298,284,297,299,166,298,441,
		0,600,302,3,302,307,300,303,301,302,601,305,305,608,314,303,304,306,305,312,307,301,306,308,4,307,310,15,310,323,308,311,309,310,312,321,306,313,311,312,314,319,304,315,313,314,609,317,317,624,349,315,316,318,317,347,319,313,318,320,319,345,321,311,320,322,321,343,323,309,322,324,16,323,341,
		200,327,350,327,332,353,325,328,326,201,330,327,208,339,330,328,329,331,330,337,332,326,331,333,332,335,354,335,348,365,333,336,334,335,337,346,331,338,336,337,339,344,329,340,338,209,342,339,224,324,342,340,341,343,322,344,342,338,343,345,320,346,344,336,345,347,318,348,346,334,347,349,316,366,348,
		325,352,375,352,357,378,350,353,351,326,355,352,333,364,355,353,354,356,355,362,357,351,356,358,357,360,379,360,373,390,358,361,359,360,362,371,356,363,361,362,364,369,354,365,363,334,367,364,349,524,367,365,366,368,367,522,369,363,368,370,369,520,371,361,370,372,371,518,373,359,372,374,373,516,391,
		225,350,377,228,377,382,375,378,376,351,380,377,358,389,380,378,379,381,380,387,382,376,381,383,229,382,385,240,385,398,383,386,384,385,387,396,381,388,386,387,389,394,379,390,388,359,392,389,374,424,392,390,391,393,392,422,394,388,393,395,394,420,396,386,395,397,396,418,398,384,397,399,241,398,416,
		402,550,425,402,407,553,400,403,401,402,426,405,405,433,414,403,404,406,405,412,407,401,406,408,407,410,554,410,423,565,408,411,409,410,412,421,406,413,411,412,414,419,404,415,413,414,434,417,399,417,449,415,416,418,397,419,417,413,418,420,395,421,419,411,420,422,393,423,421,409,422,424,391,566,423,
		400,450,427,403,427,432,425,428,426,427,451,430,430,458,439,428,429,431,430,437,432,426,431,433,404,432,435,415,435,448,433,436,434,435,437,446,431,438,436,437,439,444,429,440,438,439,459,442,299,442,474,440,441,443,297,444,442,438,443,445,295,446,444,436,445,447,293,448,446,434,447,449,291,416,448,
		425,575,452,428,452,457,450,453,451,452,576,455,455,583,464,453,454,456,455,462,457,451,456,458,429,457,460,440,460,473,458,461,459,460,462,471,456,463,461,462,464,469,454,465,463,464,584,467,467,599,499,465,466,468,467,497,469,463,468,470,469,495,471,461,470,472,471,493,473,459,472,474,441,473,491,
		175,477,775,477,482,778,475,478,476,176,480,477,183,489,480,478,479,481,480,487,482,476,481,483,482,485,779,485,498,790,483,486,484,485,487,496,481,488,486,487,489,494,479,490,488,184,492,489,199,474,492,490,491,493,472,494,492,488,493,495,470,496,494,486,495,497,468,498,496,484,497,499,466,791,498,
		502,625,525,502,507,628,500,503,501,502,526,505,505,533,514,503,504,506,505,512,507,501,506,508,507,510,629,510,523,640,508,511,509,510,512,521,506,513,511,512,514,519,504,515,513,514,534,517,374,517,549,515,516,518,372,519,517,513,518,520,370,521,519,511,520,522,368,523,521,509,522,524,366,641,523,
		500,675,527,503,527,532,525,528,526,527,676,530,530,683,539,528,529,531,530,537,532,526,531,533,504,532,535,515,535,548,533,536,534,535,537,546,531,538,536,537,539,544,529,540,538,539,684,542,542,699,574,540,541,543,542,572,544,538,543,545,544,570,546,536,545,547,546,568,548,534,547,549,516,548,566,
		400,552,575,552,557,578,550,553,551,401,555,552,408,564,555,553,554,556,555,562,557,551,556,558,557,560,579,560,573,590,558,561,559,560,562,571,556,563,561,562,564,569,554,565,563,409,567,564,424,549,567,565,566,568,547,569,567,563,568,570,545,571,569,561,570,572,543,573,571,559,572,574,541,591,573,
		450,550,577,453,577,582,575,578,576,551,580,577,558,589,580,578,579,581,580,587,582,576,581,583,454,582,585,465,585,598,583,586,584,585,587,596,581,588,586,587,589,594,579,590,588,559,592,589,574,749,592,590,591,593,592,747,594,588,593,595,594,745,596,586,595,597,596,743,598,584,597,599,466,598,741,
		300,900,602,303,602,607,600,603,601,602,901,605,605,908,614,603,604,606,605,612,607,601,606,608,304,607,610,315,610,623,608,611,609,610,612,621,606,613,611,612,614,619,604,615,613,614,909,617,617,924,649,615,616,618,617,647,619,613,618,620,619,645,621,611,620,622,621,643,623,609,622,624,316,623,641,
		500,627,650,627,632,653,625,628,626,501,630,627,508,639,630,628,629,631,630,637,632,626,631,633,632,635,654,635,648,665,633,636,634,635,637,646,631,638,636,637,639,644,629,640,638,509,642,639,524,624,642,640,641,643,622,644,642,638,643,645,620,646,644,636,645,647,618,648,646,634,647,649,616,666,648,
		625,652,675,652,657,678,650,653,651,626,655,652,633,664,655,653,654,656,655,662,657,651,656,658,657,660,679,660,673,690,658,661,659,660,662,671,656,663,661,662,664,669,654,665,663,634,667,664,649,824,667,665,666,668,667,822,669,663,668,670,669,820,671,661,670,672,671,818,673,659,672,674,673,816,691,
		525,650,677,528,677,682,675,678,676,651,680,677,658,689,680,678,679,681,680,687,682,676,681,683,529,682,685,540,685,698,683,686,684,685,687,696,681,688,686,687,689,694,679,690,688,659,692,689,674,724,692,690,691,693,692,722,694,688,693,695,694,720,696,686,695,697,696,718,698,684,697,699,541,698,716,
		702,850,725,702,707,853,700,703,701,702,726,705,705,733,714,703,704,706,705,712,707,701,706,708,707,710,854,710,723,865,708,711,709,710,712,721,706,713,711,712,714,719,704,715,713,714,734,717,699,717,749,715,716,718,697,719,717,713,718,720,695,721,719,711,720,722,693,723,721,709,722,724,691,866,723,
		700,750,727,703,727,732,725,728,726,727,751,730,730,758,739,728,729,731,730,737,732,726,731,733,704,732,735,715,735,748,733,736,734,735,737,746,731,738,736,737,739,744,729,740,738,739,759,742,599,742,774,740,741,743,597,744,742,738,743,745,595,746,744,736,745,747,593,748,746,734,747,749,591,716,748,
		725,875,752,728,752,757,750,753,751,752,876,755,755,883,764,753,754,756,755,762,757,751,756,758,729,757,760,740,760,773,758,761,759,760,762,771,756,763,761,762,764,769,754,765,763,764,884,767,767,899,799,765,766,768,767,797,769,763,768,770,769,795,771,761,770,772,771,793,773,759,772,774,741,773,791,
		475,777,1075,777,782,1078,775,778,776,476,780,777,483,789,780,778,779,781,780,787,782,776,781,783,782,785,1079,785,798,1090,783,786,784,785,787,796,781,788,786,787,789,794,779,790,788,484,792,789,499,774,792,790,791,793,772,794,792,788,793,795,770,796,794,786,795,797,768,798,796,784,797,799,766,1091,798,
		802,925,825,802,807,928,800,803,801,802,826,805,805,833,814,803,804,806,805,812,807,801,806,808,807,810,929,810,823,940,808,811,809,810,812,821,806,813,811,812,814,819,804,815,813,814,834,817,674,817,849,815,816,818,672,819,817,813,818,820,670,821,819,811,820,822,668,823,821,809,822,824,666,941,823,
		800,975,827,803,827,832,825,828,826,827,976,830,830,983,839,828,829,831,830,837,832,826,831,833,804,832,835,815,835,848,833,836,834,835,837,846,831,838,836,837,839,844,829,840,838,839,984,842,842,999,874,840,841,843,842,872,844,838,843,845,844,870,846,836,845,847,846,868,848,834,847,849,816,848,866,
		700,852,875,852,857,878,850,853,851,701,855,852,708,864,855,853,854,856,855,862,857,851,856,858,857,860,879,860,873,890,858,861,859,860,862,871,856,863,861,862,864,869,854,865,863,709,867,864,724,849,867,865,866,868,847,869,867,863,868,870,845,871,869,861,870,872,843,873,871,859,872,874,841,891,873,
		750,850,877,753,877,882,875,878,876,851,880,877,858,889,880,878,879,881,880,887,882,876,881,883,754,882,885,765,885,898,883,886,884,885,887,896,881,888,886,887,889,894,879,890,888,859,892,889,874,1049,892,890,891,893,892,1047,894,888,893,895,894,1045,896,886,895,897,896,1043,898,884,897,899,766,898,1041,
		600,1200,902,603,902,907,900,903,901,902,1201,905,905,1208,914,903,904,906,905,912,907,901,906,908,604,907,910,615,910,923,908,911,909,910,912,921,906,913,911,912,914,919,904,915,913,914,1209,917,917,1224,949,915,916,918,917,947,919,913,918,920,919,945,921,911,920,922,921,943,923,909,922,924,616,923,941,
		800,927,950,927,932,953,925,928,926,801,930,927,808,939,930,928,929,931,930,937,932,926,931,933,932,935,954,935,948,965,933,936,934,935,937,946,931,938,936,937,939,944,929,940,938,809,942,939,824,924,942,940,941,943,922,944,942,938,943,945,920,946,944,936,945,947,918,948,946,934,947,949,916,966,948,
		925,952,975,952,957,978,950,953,951,926,955,952,933,964,955,953,954,956,955,962,957,951,956,958,957,960,979,960,973,990,958,961,959,960,962,971,956,963,961,962,964,969,954,965,963,934,967,964,949,1124,967,965,966,968,967,1122,969,963,968,970,969,1120,971,961,970,972,971,1118,973,959,972,974,973,1116,991,
		825,950,977,828,977,982,975,978,976,951,980,977,958,989,980,978,979,981,980,987,982,976,981,983,829,982,985,840,985,998,983,986,984,985,987,996,981,988,986,987,989,994,979,990,988,959,992,989,974,1024,992,990,991,993,992,1022,994,988,993,995,994,1020,996,986,995,997,996,1018,998,984,997,999,841,998,1016,
		1002,1150,1025,1002,1007,1153,1000,1003,1001,1002,1026,1005,1005,1033,1014,1003,1004,1006,1005,1012,1007,1001,1006,1008,1007,1010,1154,1010,1023,1165,1008,1011,1009,1010,1012,1021,1006,1013,1011,1012,1014,1019,1004,1015,1013,1014,1034,1017,999,1017,1049,1015,1016,1018,997,1019,1017,1013,1018,1020,995,1021,1019,1011,1020,1022,993,1023,1021,1009,1022,1024,991,1166,1023,
		1000,1050,1027,1003,1027,1032,1025,1028,1026,1027,1051,1030,1030,1058,1039,1028,1029,1031,1030,1037,1032,1026,1031,1033,1004,1032,1035,1015,1035,1048,1033,1036,1034,1035,1037,1046,1031,1038,1036,1037,1039,1044,1029,1040,1038,1039,1059,1042,899,1042,1074,1040,1041,1043,897,1044,1042,1038,1043,1045,895,1046,1044,1036,1045,1047,893,1048,1046,1034,1047,1049,891,1016,1048,
		1025,1175,1052,1028,1052,1057,1050,1053,1051,1052,1176,1055,1055,1183,1064,1053,1054,1056,1055,1062,1057,1051,1056,1058,1029,1057,1060,1040,1060,1073,1058,1061,1059,1060,1062,1071,1056,1063,1061,1062,1064,1069,1054,1065,1063,1064,1184,1067,1067,1199,1099,1065,1066,1068,1067,1097,1069,1063,1068,1070,1069,1095,1071,1061,1070,1072,1071,1093,1073,1059,1072,1074,1041,1073,1091,
		775,1077,1375,1077,1082,1378,1075,1078,1076,776,1080,1077,783,1089,1080,1078,1079,1081,1080,1087,1082,1076,1081,1083,1082,1085,1379,1085,1098,1390,1083,1086,1084,1085,1087,1096,1081,1088,1086,1087,1089,1094,1079,1090,1088,784,1092,1089,799,1074,1092,1090,1091,1093,1072,1094,1092,1088,1093,1095,1070,1096,1094,1086,1095,1097,1068,1098,1096,1084,1097,1099,1066,1391,1098,
		1102,1225,1125,1102,1107,1228,1100,1103,1101,1102,1126,1105,1105,1133,1114,1103,1104,1106,1105,1112,1107,1101,1106,1108,1107,1110,1229,1110,1123,1240,1108,1111,1109,1110,1112,1121,1106,1113,1111,1112,1114,1119,1104,1115,1113,1114,1134,1117,974,1117,1149,1115,1116,1118,972,1119,1117,1113,1118,1120,970,1121,1119,1111,1120,1122,968,1123,1121,1109,1122,1124,966,1241,1123,
		1100,1275,1127,1103,1127,1132,1125,1128,1126,1127,1276,1130,1130,1283,1139,1128,1129,1131,1130,1137,1132,1126,1131,1133,1104,1132,1135,1115,1135,1148,1133,1136,1134,1135,1137,1146,1131,1138,1136,1137,1139,1144,1129,1140,1138,1139,1284,1142,1142,1299,1174,1140,1141,1143,1142,1172,1144,1138,1143,1145,1144,1170,1146,1136,1145,1147,1146,1168,1148,1134,1147,1149,1116,1148,1166,
		1000,1152,1175,1152,1157,1178,1150,1153,1151,1001,1155,1152,1008,1164,1155,1153,1154,1156,1155,1162,1157,1151,1156,1158,1157,1160,1179,1160,1173,1190,1158,1161,1159,1160,1162,1171,1156,1163,1161,1162,1164,1169,1154,1165,1163,1009,1167,1164,1024,1149,1167,1165,1166,1168,1147,1169,1167,1163,1168,1170,1145,1171,1169,1161,1170,1172,1143,1173,1171,1159,1172,1174,1141,1191,1173,
		1050,1150,1177,1053,1177,1182,1175,1178,1176,1151,1180,1177,1158,1189,1180,1178,1179,1181,1180,1187,1182,1176,1181,1183,1054,1182,1185,1065,1185,1198,1183,1186,1184,1185,1187,1196,1181,1188,1186,1187,1189,1194,1179,1190,1188,1159,1192,1189,1174,1349,1192,1190,1191,1193,1192,1347,1194,1188,1193,1195,1194,1345,1196,1186,1195,1197,1196,1343,1198,1184,1197,1199,1066,1198,1341,
		0,1202,900,903,1202,1207,1200,1203,1201,1,1205,1202,8,1214,1205,1203,1204,1206,1205,1212,1207,1201,1206,1208,904,1207,1210,915,1210,1223,1208,1211,1209,1210,1212,1221,1206,1213,1211,1212,1214,1219,1204,1215,1213,9,1217,1214,24,1249,1217,1215,1216,1218,1217,1247,1219,1213,1218,1220,1219,1245,1221,1211,1220,1222,1221,1243,1223,1209,1222,1224,916,1223,1241,
		1100,1227,1250,1227,1232,1253,1225,1228,1226,1101,1230,1227,1108,1239,1230,1228,1229,1231,1230,1237,1232,1226,1231,1233,1232,1235,1254,1235,1248,1265,1233,1236,1234,1235,1237,1246,1231,1238,1236,1237,1239,1244,1229,1240,1238,1109,1242,1239,1124,1224,1242,1240,1241,1243,1222,1244,1242,1238,1243,1245,1220,1246,1244,1236,1245,1247,1218,1248,1246,1234,1247,1249,1216,1266,1248,
		1225,1252,1275,1252,1257,1278,1250,1253,1251,1226,1255,1252,1233,1264,1255,1253,1254,1256,1255,1262,1257,1251,1256,1258,1257,1260,1279,1260,1273,1290,1258,1261,1259,1260,1262,1271,1256,1263,1261,1262,1264,1269,1254,1265,1263,1234,1267,1264,1249,1424,1267,1265,1266,1268,1267,1422,1269,1263,1268,1270,1269,1420,1271,1261,1270,1272,1271,1418,1273,1259,1272,1274,1273,1416,1291,
		1125,1250,1277,1128,1277,1282,1275,1278,1276,1251,1280,1277,1258,1289,1280,1278,1279,1281,1280,1287,1282,1276,1281,1283,1129,1282,1285,1140,1285,1298,1283,1286,1284,1285,1287,1296,1281,1288,1286,1287,1289,1294,1279,1290,1288,1259,1292,1289,1274,1324,1292,1290,1291,1293,1292,1322,1294,1288,1293,1295,1294,1320,1296,1286,1295,1297,1296,1318,1298,1284,1297,1299,1141,1298,1316,
		1302,1450,1325,1302,1307,1453,1300,1303,1301,1302,1326,1305,1305,1333,1314,1303,1304,1306,1305,1312,1307,1301,1306,1308,1307,1310,1454,1310,1323,1465,1308,1311,1309,1310,1312,1321,1306,1313,1311,1312,1314,1319,1304,1315,1313,1314,1334,1317,1299,1317,1349,1315,1316,1318,1297,1319,1317,1313,1318,1320,1295,1321,1319,1311,1320,1322,1293,1323,1321,1309,1322,1324,1291,1466,1323,
		1300,1350,1327,1303,1327,1332,1325,1328,1326,1327,1351,1330,1330,1358,1339,1328,1329,1331,1330,1337,1332,1326,1331,1333,1304,1332,1335,1315,1335,1348,1333,1336,1334,1335,1337,1346,1331,1338,1336,1337,1339,1344,1329,1340,1338,1339,1359,1342,1199,1342,1374,1340,1341,1343,1197,1344,1342,1338,1343,1345,1195,1346,1344,1336,1345,1347,1193,1348,1346,1334,1347,1349,1191,1316,1348,
		1325,1475,1352,1328,1352,1357,1350,1353,1351,1352,1476,1355,1355,1483,1364,1353,1354,1356,1355,1362,1357,1351,1356,1358,1329,1357,1360,1340,1360,1373,1358,1361,1359,1360,1362,1371,1356,1363,1361,1362,1364,1369,1354,1365,1363,1364,1484,1367,1367,1499,1399,1365,1366,1368,1367,1397,1369,1363,1368,1370,1369,1395,1371,1361,1370,1372,1371,1393,1373,1359,1372,1374,1341,1373,1391,
		175,1075,1377,178,1377,1382,1375,1378,1376,1076,1380,1377,1083,1389,1380,1378,1379,1381,1380,1387,1382,1376,1381,1383,179,1382,1385,190,1385,1398,1383,1386,1384,1385,1387,1396,1381,1388,1386,1387,1389,1394,1379,1390,1388,1084,1392,1389,1099,1374,1392,1390,1391,1393,1372,1394,1392,1388,1393,1395,1370,1396,1394,1386,1395,1397,1368,1398,1396,1384,1397,1399,191,1398,1366,
		25,1425,1402,28,1402,1407,1400,1403,1401,1402,1426,1405,1405,1433,1414,1403,1404,1406,1405,1412,1407,1401,1406,1408,29,1407,1410,40,1410,1423,1408,1411,1409,1410,1412,1421,1406,1413,1411,1412,1414,1419,1404,1415,1413,1414,1434,1417,1274,1417,1449,1415,1416,1418,1272,1419,1417,1413,1418,1420,1270,1421,1419,1411,1420,1422,1268,1423,1421,1409,1422,1424,41,1423,1266,
		75,1427,1400,1403,1427,1432,1425,1428,1426,76,1430,1427,83,1439,1430,1428,1429,1431,1430,1437,1432,1426,1431,1433,1404,1432,1435,1415,1435,1448,1433,1436,1434,1435,1437,1446,1431,1438,1436,1437,1439,1444,1429,1440,1438,84,1442,1439,99,1474,1442,1440,1441,1443,1442,1472,1444,1438,1443,1445,1444,1470,1446,1436,1445,1447,1446,1468,1448,1434,1447,1449,1416,1448,1466,
		1300,1452,1475,1452,1457,1478,1450,1453,1451,1301,1455,1452,1308,1464,1455,1453,1454,1456,1455,1462,1457,1451,1456,1458,1457,1460,1479,1460,1473,1490,1458,1461,1459,1460,1462,1471,1456,1463,1461,1462,1464,1469,1454,1465,1463,1309,1467,1464,1324,1449,1467,1465,1466,1468,1447,1469,1467,1463,1468,1470,1445,1471,1469,1461,1470,1472,1443,1473,1471,1459,1472,1474,1441,1491,1473,
		1350,1450,1477,1353,1477,1482,1475,1478,1476,1451,1480,1477,1458,1489,1480,1478,1479,1481,1480,1487,1482,1476,1481,1483,1354,1482,1485,1365,1485,1498,1483,1486,1484,1485,1487,1496,1481,1488,1486,1487,1489,1494,1479,1490,1488,1459,1492,1489,149,1492,1474,1490,1491,1493,147,1494,1492,1488,1493,1495,145,1496,1494,1486,1495,1497,143,1498,1496,1484,1497,1499,141,1366,1498,
	};



}
