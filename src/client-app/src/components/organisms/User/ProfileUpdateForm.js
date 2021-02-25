import React, { Fragment, useState } from "react";
import styled from "styled-components";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { PanelBody } from "../../atoms/Panels";
import { ButtonPrimary } from "../../../components/atoms/Buttons/Buttons";
import LabelAndTextbox from "../../molecules/InfoWithLabels/LabelAndTextbox";
import { checkValidity } from "../../../utils/Validity";
import { PanelFooter } from "../../../components/atoms/Panels";
import { QuaternaryDarkHeading } from "../../atoms/Heading";

const MainPanel = styled(PanelBody)`
  border-radius: ${(p) => p.theme.borderRadius.normal};
  box-shadow: ${(p) => p.theme.shadow.BoxShadow};
  margin-bottom: ${(p) => p.theme.size.normal};
  background-color: ${(p) => p.theme.color.whiteBg};
`;

const FormGroup = styled.div`
  margin-bottom: ${(p) => p.theme.size.exTiny};
  border-bottom: 1px solid ${(p) => p.theme.color.secondaryDivide};
`;

const Heading = styled(QuaternaryDarkHeading)`
  margin-bottom: ${(p) => p.theme.size.distance};
  margin-left: ${(p) => p.theme.size.exTiny};
`;

const FormFooter = styled(PanelFooter)`
  padding-left: 0;
  padding-right: 0;
`;

export default (props) => {
  const { userInfo } = props;

  let model = {
    displayName: {
      value: userInfo.displayName,
      validation: {
        isRequired: true,
      },
      isValid: !!userInfo.displayName,
    },
    lastname: {
      value: userInfo.lastname,
      validation: {
        isRequired: true,
      },
      isValid: !!userInfo.lastname,
    },
    firstname: {
      value: userInfo.firstname,
      validation: {
        isRequired: true,
      },
      isValid: !!userInfo.firstname,
    },
  };

  const [formData, setFromData] = useState(model);

  const onTextboxChange = (e) => {
    let userData = formData || {};
    const { name, value } = e.target;

    // Validate when input
    userData[name].isValid = checkValidity(userData, value, name);
    userData[name].value = value;

    setFromData({
      ...userData,
    });
  };

  const checkIsFormValid = () => {
    let isFormValid = false;
    for (let formIdentifier in formData) {
      isFormValid = formData[formIdentifier].isValid;
      if (!isFormValid) {
        break;
      }
    }

    return isFormValid;
  };

  const onUpdate = (e) => {
    e.preventDefault();

    let isFormValid = true;
    for (let formIdentifier in formData) {
      isFormValid = formData[formIdentifier].isValid && isFormValid;

      if (!isFormValid) {
        props.showValidationError(
          "Something went wrong with your input",
          "Something went wrong with your information, please check and input again",
          "error"
        );
      }
    }

    if (!!isFormValid) {
      const profileData = {};
      for (const formIdentifier in formData) {
        profileData[formIdentifier] = formData[formIdentifier].value;
      }

      props.onUpdate(profileData);
    }
  };

  const { displayName, firstname, lastname } = formData;
  const isFormValid = checkIsFormValid();

  return (
    <Fragment>
      <Heading>Update personal information</Heading>
      <MainPanel>
        <form onSubmit={(e) => onUpdate(e)} method="POST">
          {userInfo ? (
            <Fragment>
              <FormGroup>
                <LabelAndTextbox
                  label="Lastname"
                  name="lastname"
                  value={lastname.value}
                  onChange={onTextboxChange}
                />
              </FormGroup>
              <FormGroup>
                <LabelAndTextbox
                  label="Firstname"
                  name="firstname"
                  value={firstname.value}
                  onChange={onTextboxChange}
                />
              </FormGroup>
              <FormGroup>
                <LabelAndTextbox
                  label="Display Name"
                  name="displayName"
                  value={displayName.value}
                  onChange={onTextboxChange}
                />
              </FormGroup>
            </Fragment>
          ) : null}
          <FormFooter>
            <ButtonPrimary
              type="submit"
              size="xs"
              disabled={!props.isFormEnabled || !isFormValid}
            >
              <FontAwesomeIcon icon="pencil-alt" className="me-1" />
              Update
            </ButtonPrimary>
          </FormFooter>
        </form>
      </MainPanel>
    </Fragment>
  );
};
