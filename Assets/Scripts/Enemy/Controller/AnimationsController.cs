using System;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public abstract class AnimationsController : MonoBehaviour
{
    [SerializeField] protected Animator _animator;
    [SerializeField] private bool _animateWhenRun = true;
    protected static int MovingHash = Animator.StringToHash("IsMoving");
    protected static int AttackHash = Animator.StringToHash("Attack");
    protected static int HitHash = Animator.StringToHash("Hit");
    protected static int IsDeadHash = Animator.StringToHash("IsDead");


    private Vector3 _lastPosition;
    private Quaternion _lastRotation;

    private void Awake()
    {
        _lastPosition = transform.position;
        _lastRotation = transform.rotation;        
    }

    private void Start()
    {
        Inicializar();
    }

    public virtual void Inicializar()
    {
        throw new NotImplementedException();
    }
    private void Update()
    {
        if (_animateWhenRun)
        {
            var tr = transform;
            var position = tr.position;

            if (_lastPosition.x != position.x || _lastPosition.z != position.z)
            {
                SetMovingState(true);

                var dirrection = position - _lastPosition;
                dirrection.y = 0;
                _lastRotation = Quaternion.LookRotation(dirrection);
            }
            else
            {
                SetMovingState(false);
            }
            transform.rotation = Quaternion.Lerp(transform.rotation, _lastRotation, 10f * Time.deltaTime);
            _lastPosition = position;
        }


    }

    private void OnValidate()
    {
        if (_animator == null)
        {
            _animator = GetComponent<Animator>();
        }
    }

    public virtual void SetMovingState(bool val)
    {
        _animator.SetBool(MovingHash, val);
    }
    public virtual void SetDead()
    {
        _animator.SetBool(IsDeadHash, true);
    }

    public virtual void ClearAll(){}

    public virtual void ClearDead()
    {
        _animator.SetBool(IsDeadHash, false);
        _animator.Play("Idle");
    }
    public virtual void Attack()
    {
        _animator.SetTrigger(AttackHash);
    }
    public virtual void Hit()
    {
        _animator.SetTrigger(HitHash);
    }

    public virtual void SetAlert(bool val)
    {
        
    }

    public virtual bool IsMoving
    {
        get { return _animator.GetBool(MovingHash); }
    }
}