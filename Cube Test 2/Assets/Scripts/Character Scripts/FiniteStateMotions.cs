using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Transition
{
    //represents non-existing transition in system
    NullTransition = 0,
    ResetToNeutral, NeutralDown, DownToDB, DBToBack, BackToSpecial2, DownToDF, DFToFront, FrontToSpecial1, FrontToSuper
}

public enum StateID
{
    //this ID represents non-existant state in system
    NullStateID = 0,
    Neutral, Down, DownBack, Back, Special2, DownFront, Front, Special1, Super
}


/// <summary>
/// This class represents the States in the Finite State System.
/// Each state has a Dictionary with pairs (transition-state) showing
/// which state the FSM should be if a transition is fired while this state
/// is the current state.
/// Method Reason is used to determine which transition should be fired .
/// Method Act has the code to perform the actions the NPC is supposed do if it's on this state.
/// </summary>
public class FiniteStateMachineSystem : MonoBehaviour{

    protected Dictionary<Transition, StateID> map;
    protected StateID stateID;
	public StateID ID { get { return stateID; } }
    private static int frameWindow;
    public void Start()
    {
        map = new Dictionary<Transition, StateID>();
        frameWindow = 0;
        StartCoroutine(CountTime());
    }

    private void ResetMachine()
    {
        frameWindow = 0;
        map = new Dictionary<Transition, StateID>();
        StopAllCoroutines();
    }


    private IEnumerator CountTime()
    {
        yield return new WaitForSeconds(1f);
        if (stateID == StateID.Special1 || stateID == StateID.Special2 || stateID == StateID.Super)
        {
            //send start flag to animation
        }
        ResetMachine();
    }

    public void AddTransition(Transition trans, StateID id)
    {
        //checks for invalid arguments
        if (trans == Transition.NullTransition)
        {
            Debug.LogError("FiniteStateMotion ERROR: NullTransition is not allowed for a real transition");
            return;
        }

        if(id == StateID.NullStateID)
        {
            Debug.LogError("FiniteStateMotion ERROR: State " + stateID.ToString() + " already has transition " + trans.ToString() +
                           "Impossible to assign to another state");
            return;
        }

        //Deterministic FSM
        // check current transition was already inside the maap
        if(map.ContainsKey(trans))
        {
            Debug.LogError("FiniteStateMotion ERROR: State " + stateID.ToString() + " already has transition " + trans.ToString() +
                           "Impossible to assign to another state");
            return;
        }

        switch (stateID)
        {
            case StateID.Neutral:
                switch (trans)
                {
                    case Transition.NeutralDown:
                        StartCoroutine(CountTime());
                        map.Add(trans, id);
                        break;

                    default:
                        ResetMachine();
                        break;
                }
                break;

            case StateID.Down:
                switch (trans)
                {
                    case Transition.DownToDB:
                        map.Add(trans, id);
                        break;

                    case Transition.DownToDF:
                        map.Add(trans, id);
                        break;

                    default:
                        ResetMachine();
                        break;
                }
                break;

            case StateID.DownBack:
                switch (trans)
                {
                    case Transition.DBToBack:
                        map.Add(trans, id);
                        break;

                    default:
                        ResetMachine();
                        break;
                }
                break;

            case StateID.Back:
                switch (trans)
                {
                    case Transition.BackToSpecial2:
                        map.Add(trans, id);
                        break;

                    default:
                        ResetMachine();
                        break;
                }
                break;

            case StateID.DownFront:
                switch (trans)
                {
                    case Transition.DFToFront:
                        map.Add(trans, id);
                        break;

                    default:
                        ResetMachine();
                        break;
                }
                break;

            case StateID.Front:
                switch (trans)
                {
                    case Transition.FrontToSpecial1:
                        map.Add(trans, id);
                        break;

                    case Transition.FrontToSuper:
                        map.Add(trans, id);
                        break;

                    default:
                        ResetMachine();
                        break;
                }
                break;
        }

    }


    public StateID GetOutputState(Transition trans)
    {
        //check if map has transition
        if (map.ContainsKey(trans))
        {
            return map[trans];
        }
        return StateID.NullStateID;
    }

    ////////// NAO SEI O QUE FAZER COM O QUE TENHO ABAIXO DISTO


    /// <summary>
    /// This method is used to set up the State condition before entering it.
    /// It is called automatically by the FSMSystem class before assigning it
    /// to the current state.
    /// </summary>
    public virtual void DoBeforeEntering() { }

    /// <summary>
    /// This method is used to make anything necessary, as reseting variables
    /// before the FSMSystem changes to another one. It is called automatically
    /// by the FSMSystem before changing to a new state.
    /// </summary>
    public virtual void DoBeforeLeaving() { }

    /// <summary>
    /// This method decides if the state should transition to another on its list
    /// NPC is a reference to the object that is controlled by this class
    /// </summary>
    //public abstract void Reason(GameObject player, GameObject npc);

    /// <summary>
    /// This method controls the behavior of the NPC in the game World.
    /// Every action, movement or communication the NPC does should be placed here
    /// NPC is a reference to the object that is controlled by this class
    /// </summary>
    //public abstract void Act(GameObject player, GameObject npc);

} // class FiniteStateMotion


/// <summary>
/// FSMSystem class represents the Finite State Machine class.
///  It has a List with the States the NPC has and methods to add,
///  delete a state, and to change the current state the Machine is on.
/// </summary>
public class FiniteStateMachineClass
{
    private List<FiniteStateMachineSystem> states;

    // The only way one can change the state of the FSM is by performing a transition
    // Don't change the CurrentState directly
    private StateID currentStateID;
    public StateID CurrentStateID { get { return currentStateID; } }
    private FiniteStateMachineSystem currentState;
    public FiniteStateMachineSystem CurrentState { get { return currentState; } }

    public FiniteStateMachineClass()
    {
        states = new List<FiniteStateMachineSystem>();
    }

    /// <summary>
    /// This method places new states inside the FSM,
    /// or prints an ERROR message if the state was already inside the List.
    /// First state added is also the initial state.
    /// </summary>
    public void AddState(FiniteStateMachineSystem s)
    {
        // Check for Null reference before deleting
        if (s == null)
        {
            Debug.LogError("FSM ERROR: Null reference is not allowed");
        }

        // First State inserted is also the Initial state,
        //   the state the machine is in when the simulation begins
        if (states.Count == 0)
        {
            states.Add(s);
            currentState = s;
            currentStateID = s.ID;
            return;
        }

        // Add the state to the List if it's not inside it
        foreach (FiniteStateMachineSystem state in states)
        {
            if (state.ID == s.ID)
            {
                Debug.LogError("FSM ERROR: Impossible to add state " + s.ID.ToString() +
                               " because state has already been added");
                return;
            }
        }
        states.Add(s);
    }

    /// <summary>
    /// This method delete a state from the FSM List if it exists, 
    ///   or prints an ERROR message if the state was not on the List.
    /// </summary>
    public void DeleteState(StateID id)
    {
        // Check for NullState before deleting
        if (id == StateID.NullStateID)
        {
            Debug.LogError("FSM ERROR: NullStateID is not allowed for a real state");
            return;
        }

        // Search the List and delete the state if it's inside it
        foreach (FiniteStateMachineSystem state in states)
        {
            if (state.ID == id)
            {
                states.Remove(state);
                return;
            }
        }
        Debug.LogError("FSM ERROR: Impossible to delete state " + id.ToString() +
                       ". It was not on the list of states");
    }

    /// <summary>
    /// This method tries to change the state the FSM is in based on
    /// the current state and the transition passed. If current state
    ///  doesn't have a target state for the transition passed, 
    /// an ERROR message is printed.
    /// </summary>
    public void PerformTransition(Transition trans)
    {
        // Check for NullTransition before changing the current state
        if (trans == Transition.NullTransition)
        {
            Debug.LogError("FSM ERROR: NullTransition is not allowed for a real transition");
            return;
        }

        // Check if the currentState has the transition passed as argument
        StateID id = currentState.GetOutputState(trans);
        if (id == StateID.NullStateID)
        {
            Debug.LogError("FSM ERROR: State " + currentStateID.ToString() + " does not have a target state " +
                           " for transition " + trans.ToString());
            return;
        }

        // Update the currentStateID and currentState		
        currentStateID = id;
        foreach (FiniteStateMachineSystem state in states)
        {
            if (state.ID == currentStateID)
            {
                // Do the post processing of the state before setting the new one
                currentState.DoBeforeLeaving();

                currentState = state;

                // Reset the state to its desired condition before it can reason or act
                currentState.DoBeforeEntering();
                break;
            }
        }

    } // PerformTransition()

} //class FSMSystem
