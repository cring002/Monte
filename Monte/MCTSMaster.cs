﻿using System;
using System.Collections.Generic;
using System.Threading;
using System.Xml;

namespace Monte
{
	abstract public class MCTSMaster
	{
		protected double thinkingTime;
		protected double exploreWeight;
		protected int maxRollout;
		protected System.Random randGen = new System.Random (1);
		protected Thread aiThread;

		public bool done;
		public bool started;
		public AIState next;

		public MCTSMaster (double _thinkingTime, double _exploreWeight, int _maxRollout)
		{
			thinkingTime = _thinkingTime;
			exploreWeight = _exploreWeight;
			maxRollout = _maxRollout;
		}

		public MCTSMaster ()
		{
			parseXML ("Assets/Monte/DefaultSettings.xml");
			//thinkingTime = 5.0;
			//exploreWeight = 1.45f;
			//maxRollout = 64;
		}

		public MCTSMaster (String fileName)
		{
			try{
				parseXML (fileName);
			} 
			catch {
				throw;
			}
			//thinkingTime = 5.0;
			//exploreWeight = 1.45f;
			//maxRollout = 64;
		}

		private void parseXML(string filePath)
		{
			try{
				XmlDocument settings = new XmlDocument ();
				settings.Load(filePath); 

				XmlNode root = settings.DocumentElement;

				XmlNode node = root.SelectSingleNode("descendant::MCTSSettings");

				thinkingTime = double.Parse(node.Attributes.GetNamedItem("ThinkingTime").Value);
				exploreWeight = double.Parse(node.Attributes.GetNamedItem("ExploreWeight").Value);
				maxRollout = int.Parse(node.Attributes.GetNamedItem("MaxRollout").Value);
			} 
			catch (System.IO.FileNotFoundException e)
			{
				throw e;
			}
		}

		public void reset()
		{
			//Resets the flags (for threading purposes)
			started = false;
			done = false;
			next = null;
		    aiThread.Join();
		}

		public void run(AIState initalState)
		{
			//Make a new AI thread with this state
			aiThread = new Thread (new ThreadStart (() => mainAlgorithm(initalState)));
			//And start it.
			aiThread.Start ();
			//Set started to true
			started = true;
		}
		//Main MCTS algortim
		abstract protected void mainAlgorithm(AIState initalState);
		//Rollout function (plays random moves till it hits a termination)
		abstract protected void rollout(AIState rolloutStart);
	}
}

