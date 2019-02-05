using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Player
{
    public abstract class PlayerAttackBase : MonoBehaviour
    {
        [SerializeField] GameObject sheep;
        [SerializeField] GameObject pig;
        [SerializeField] GameObject cow;
        [SerializeField] Transform ProjectileTransform;

        protected GameObject animal;
        protected Animal animalComponent;
        protected Rigidbody animalRig;
        protected Collider animalCollider;
        protected bool makeAim;

        protected bool isMyTurn;
        public bool IsMyTurn { get { return isMyTurn; } }

        private bool animalSpent;
        public bool AnimalSpent { get { return animalSpent; } }

        public bool IhaveMissed;

        void Update()
        {
            OnUpdate();
        }

        private void OnUpdate()
        {
            if (!isMyTurn) return;

            if (makeAim)
            {
                SetAnimalTransform();

                if (CanDropAnimal())
                {
                    OnAnimalDropping();
                }
            }
            else
            {
                if (animal && animalComponent.IsGroundCollision)
                {
                    animalComponent.IsGroundCollision = false;
                    IhaveMissed = true;
                }
            }
        }

        private void SetAnimalTransform()
        {
            var pos = GetAnimalPosition();
            pos.z = 0;

            animal.transform.position = pos;
            animal.transform.rotation = Quaternion.Euler(0, 0, 0);
        }

        protected abstract Vector3 GetAnimalPosition();

        protected abstract bool CanDropAnimal();

        protected void OnAnimalDropping()
        {
            animalRig.isKinematic = false;
            animalCollider.enabled = true;
            makeAim = false;
        }

        public void TakeTurn(Projectile projectile)
        {
            IhaveMissed = false;
            isMyTurn = true;
            animalSpent = false;

            projectile.gameObject.SetActive(false);
            projectile.GetComponent<Rigidbody>().velocity = new Vector3();
            projectile.transform.position = ProjectileTransform.position;
            //projectile.HasFinished = false;
            projectile.gameObject.SetActive(true);

            OnTakeTurn();
        }

        protected virtual void OnTakeTurn()
        {

        }

        public void GiveTurn()
        {
            isMyTurn = false;
            IhaveMissed = false;
        }

        public void CreateSheep()
        {
            CreateAnimal(sheep);
        }

        public void CreatePig()
        {
            CreateAnimal(pig);
        }

        public void CreateCow()
        {
            CreateAnimal(cow);
        }

        private void CreateAnimal(GameObject createdAnimal)
        {
            if (!isMyTurn) return;
            if (animal && animal.activeSelf)
                animal.SetActive(false);
            animal = createdAnimal;
            animalComponent = animal.GetComponent<Animal>();
            //SetAnimalTransform();
            animalRig = createdAnimal.GetComponent<Rigidbody>();
            animalRig.isKinematic = true;
            animalCollider = createdAnimal.GetComponent<Collider>();
            animalCollider.enabled = false;
            createdAnimal.SetActive(true);
            makeAim = true;
            animalSpent = true;

            OnAnimalCreated();
        }

        protected virtual void OnAnimalCreated()
        {
            var audio = animal.GetComponent<AudioSource>(); //TODO звук пока только у телочки
            if (audio)
            {
                audio.Play();
            }
        }
    }
}
