using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour {

    [SerializeField] GameObject Sheep;
    [SerializeField] Transform ProjectileTransform;

    private GameObject animal;
    private Rigidbody animalRig;
    private bool makeAim;

    private bool isMyTurn;
    public bool IsMyTurn { get { return isMyTurn; } }

    public void TakeTurn(Projectile projectile)
    {
        projectile.GetComponent<Rigidbody>().velocity = new Vector3();
        projectile.transform.position = ProjectileTransform.position;
        //projectile.HasFinished = false;
        projectile.gameObject.SetActive(true);

        isMyTurn = true;
    }

    public void GiveTurn()
    {
        isMyTurn = false;
    }

    //TODO при разрушении катапульты сделать ей еще и случайное вращение
    //TODO сделать меню
    //TODO сделать остальных животных
    //TODO добавить звуков
    //TODO добавить искуственный интеллект
    //TODO добавить настройки (допустим, сложность и выключение звуков)
    //TODO на высоких скоростях животное стремно сталкивается с катапультой - то ли сквозь проходит, то ли еще что
    //TODO баг - при высоком сбросе он сам в себя попадает
    //TODO при сбросе животного оно остается видимым - херня, в принципе, но при переходе хода к другому игроку оно должно уже скрыться к хуям)
    //TODO если игрок промахнулся животным, то ход переходит следующему
    //Добавить возможность указать имена игроков (если не сложно, то у компа оно будет таким до смены на другое (иначе Player2) - типа, вбил имя какого-нибудь мудака, ха-ха)
    //TODO если здоровье закончилось, то объявляем победителя (имя_игрока Win!)
    //TODO возможность, чтобы камера до определенной границы могла скрольнуть вверх мышкой (а потом и обратно, если передумали), чтобы можно было с большей высоты сбросить зверя
    //  в таком случае она должна потом и вниз проследовать за падающим зверем

    //TODO Советы от Юльки: обучалка, можно жизни показывать (не понятен смысл происхоящего)
    //  TODO Разрушающиеся стены перед катапультами

    //TODO по поводу искуственного интеллекта: комп делает пристрелочный сброс в предполагаемой точке с предплагаемой высоты, потом тупо меняет высоту, если не помогло - точку
    //  нужен список расстояний и точек м.б. с высотами (игра компов друг с другом и запись значений при попадании в файл). Расстояния между игроками можно самому выставлять
    //  потом в коде тупо брать нужную точку, а если нет, то брать два ближайших расстояния, между которыми текущее и уже приближать значения к текущей точке
    //в случае добавления искуственного интеллекта тупо сделать вторую сцену, где второй игрок - комп. В остальном все также. Унаследоваться от игрока по возможности

    //TODO сделать версию для андроида)
    //TODO а в версии для андроида сделать возможность покупать реактивные снаряды/прицельные и т.д. Ну и животных разных. И катапульту более красивую чтоб могли купить
    void Update()
    {
        if (!isMyTurn) return;

        if (makeAim)
        {
            SetAnimalTransform();

            if (Input.GetMouseButtonDown(0))
            {
                animalRig.isKinematic = false;
                makeAim = false;
            }
        }
    }

    public void CreateSheep()
    {
        if (!isMyTurn) return;
        animal = Sheep;
        SetAnimalTransform();
        animalRig = Sheep.GetComponent<Rigidbody>();
        animalRig.isKinematic = true;
        Sheep.SetActive(true);
        makeAim = true;
    }

    void SetAnimalTransform()
    {
        var pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        pos.z = 0;

        animal.transform.position = pos;
        animal.transform.rotation = Quaternion.Euler(0, 0, 0);
    }
}
