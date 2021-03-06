# Changelog

All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [Unreleased]

- Implement different priprities' types for systems. Separate all systems into EARLY, NORMAL and LATE executed.

- Now only a single world's context is supported. In later builds there should be multiple worlds supported.

## [0.4.15] - 2020-08-31

### Added

- New integration tests were added for TinyECSUnityIntegration library

- Integration tests of TinyECSUnityIntegration library was added for Unity tests runner 

- The API of **IWorldContext** was extended with new public methods **GetSingleEntityWithAll** and **GetSingleEntityWithAny**

- A new method IWorldContext.GetUniqueEntity was added.

### Changed

- Now disposable entities can live for extra frames using a new builtin component **TEntityLifetimeComponent** 

- Two new types which are **EntityId** and **SystemId** were introduced as strongly typed versions of identifiers.

### Fixed

- Issue #31 : **DependencyInjector** throws **NullReferenceException** when **DependencyInjector.Init** is called before **WorldContext**'s instance is created

- Issue #30 : DependencyInjector class doesn't work correctly for nested views

- Issue #29: If you try to create a new entity no matter disposable or not the application is freezed

- Issue #27 : **RegisterViewSystem** initializes its views for a few times

- Issue #26 : **WorldContext.GetSingleEntityWithAll** and **WorldContext.GetSingleEntityWithAny** return null in some cases

- Issue #25 : **ComponentManager.AddComponent** throws **ComponentAlreadyExistsException** for entity which even has no component of this type

- Issue #24 : **BaseView.mLinkedEntityId** has invalid value when RegisterSubscription is invoked for **BaseStaticView**'s instances

## [0.4.14] - 2020-08-29

### Fixed

- Issue #31 : **DependencyInjector** throws **NullReferenceException** when **DependencyInjector.Init** is called before **WorldContext**'s instance is created

## [0.4.13] - 2020-08-25

### Added

- New integration tests were added for TinyECSUnityIntegration library

### Fixed

- Issue #30 : DependencyInjector class doesn't work correctly for nested views

## [0.4.12] - 2020-08-24

### Added

- Integration tests of TinyECSUnityIntegration library was added for Unity tests runner 

## [0.4.11]

### Changed

- Now disposable entities can live for extra frames using a new builtin component **TEntityLifetimeComponent** 

## [0.4.10] - 2020-08-23

### Fixed

- Issue #29: If you try to create a new entity no matter disposable or not the application is freezed

## [0.4.9] - 2020-08-19

### Fixed

- Issue #27 : **RegisterViewSystem** initializes its views for a few times

## [0.4.8] - 2020-08-18

### Fixed

- Issue #26 : **WorldContext.GetSingleEntityWithAll** and **WorldContext.GetSingleEntityWithAny** return null in some cases

## [0.4.7] - 2020-08-15

### Fixed

- Issue #25 : **ComponentManager.AddComponent** throws **ComponentAlreadyExistsException** for entity which even has no component of this type

## [0.4.6] - 2020-08-14

### Added

- The API of **IWorldContext** was extended with new public methods **GetSingleEntityWithAll** and **GetSingleEntityWithAny**

## [0.4.5] - 2020-08-13

### Fixed

- Issue #24 : **BaseView.mLinkedEntityId** has invalid value when RegisterSubscription is invoked for **BaseStaticView**'s instances

## [0.4.3] - 2020-07-27

### Changed

- Two new types which are **EntityId** and **SystemId** were introduced as strongly typed versions of identifiers.

## [0.4.2] - 2020-07-26

### Added

- A new method IWorldContext.GetUniqueEntity was added.

## [0.4.1] - 2020-07-26 (hotfix)

### Fixed

- Issue #22: **DependencyInjector** always throws **ArgumentNullException** on application's start

## [0.4.0] - 2020-07-25

### Added

- An extension method **CreateAndGetEntity** for **IWorldContext** was added. The main goal for that is to simplify
entity creation.

- A new concept of unique components was introduced. Now if a user implements his/her component from **IUniqueComponent**
then the only instance of this one will exists in the world context.

- Add a new type which is **SystemsPackage** that allows to unite a bunch of related systems together.

### Changed

- Now **DependencyInjector** supports initialization of multiple BaseView components per single GameObject, Issue #20

- Now all **reactive** systems accept all entities that are created despite execution order

### Fixed

- Exceptions handling within EventManager.Notify method was redesigned.

- Fixed an issue #19 "Intercommunicating reactive systems don't receive all messages of the frame"

- Fixed an issue #17 "An entity that was created using previously deteled one has its components"

- Fixed an issue #17

- Increased robustness of the framework


## [0.3.13] - 2020-07-23

### Added

- An extension method **CreateAndGetEntity** for **IWorldContext** was added. The main goal for that is to simplify
entity creation.

## [0.3.12] - 2020-07-21

### Added

- A new concept of unique components was introduced. Now if a user implements his/her component from **IUniqueComponent**
then the only instance of this one will exists in the world context.

## [0.3.11] - 2020-07-19

### Added

- Add a new type which is **SystemsPackage** that allows to unite a bunch of related systems together.

### Fixed 

- Exceptions handling within EventManager.Notify method was redesigned.

## [0.3.10] - 2020-07-11

### Changed

- Now **DependencyInjector** supports initialization of multiple BaseView components per single GameObject, Issue #20

## [0.3.9] - 2020-07-05

### Fixed

- Fixed an issue #19 "Intercommunicating reactive systems don't receive all messages of the frame"

### Changed

- Now all **reactive** systems accept all entities that are created despite execution order

## [0.3.8] - 2020-06-21

### Fixed

- Fixed an issue #17 "An entity that was created using previously deteled one has its components"

## [0.3.5] - 2020-06-20

### Fixed

- Fixed an issue #17

- Increased robustness of the framework

## [0.3.0] - 2019-06-12

### Added

- A components iterator that provides easy way of enumerating over all components that some entity has

- Add custom debug inspectors for WorldContextsManager, EntityObserver and SystemManagerObserver types

- Implement ToString() method for Entity type

- Add template project for Unity3D and corresponding tutorial sample, which demonstrates how to integrate TinyECS into Unity3D

### Fixed

- Fixed an issue "Created with GameObjectFactory entities don't have TViewComponent attached to them" #11

- Fixed an issue "Reactive systems don't response on to events that are generated with IUpdateSystem implementations" #6

### Changed

- Now all reactive systems are executed after all IUpdateSystems

## [0.2.21] - 2019-05-30

### Added

- A components iterator that provides easy way of enumerating over all components that some entity has

- Add custom debug inspectors for WorldContextsManager, EntityObserver and SystemManagerObserver types

- Implement ToString() method for Entity type

### Fixed

- Now you can subscribe same reference to a system in different roles. For example, as initializing system and as update system at the same time

## [0.2.0] - 2019-05-02

### Added

- A bunch of helper types for TinyECS were added into the project to implement an integration with Unity3D.

- Support of static and dynamically created views was implemented.

- A new type of entities was introduced which are disposable entities.

### Changed

- Now event manager separates types of delivering events to their listeners. Single- and broadcasting are now supported.

### Fixed

- IEntityManager.GetEntitiesWithAll's implementation was fixed. 

- EntityManager.DestroyEntity's implementation was fixed. Now it correctly destroys given entities.