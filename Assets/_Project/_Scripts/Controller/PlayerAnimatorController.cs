using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimatorController : MonoBehaviour
{

    private enum State { IDLE, WALK }

    [SerializeField] private Animator _animator;

    private float m_speed = 0f;
    private State m_state = State.IDLE;

    public void SetSpeed(float value)
    {
        m_speed = value;

        CheckState();
    }

    private void CheckState()
    {
        State state = GetNewState();

        if (m_state == state)
            return;

        m_state = state;
        
        _animator.Play($"Rogue_{m_state.ToString().ToLower()}_01");
    }

    private State GetNewState()
    {
        if (m_speed != 0f)
            return State.WALK;
        
        return State.IDLE;
    }

}
